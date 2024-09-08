using CameraHelpers;
using Purrcifer.Data.Defaults;
using System.Collections;
using UnityEngine;

namespace CameraHelpers
{
    /// <summary>
    /// Enum representing the current behaviour state of the camera. 
    /// </summary>
    public enum CameraBehaviour
    {
        TARGET_STEP,
        LOCK,
        TRANSVERSE
    }

    /// <summary>
    /// Generates a custom, lightweight bounds representation for AABB checks. 
    /// </summary>
    [System.Serializable]
    public class CameraBounds
    {
        private Vector3 transX;
        private Vector3 transY;

        public int width;
        public int height;

        public float MinX => -width / 2;
        public float MaxX => width / 2;
        public float MinZ => -height / 2;
        public float MaxZ => height / 2;

        public Vector3 TranslationX => transX;
        public Vector3 TranslationZ => transY;

        public CameraBounds()
        {
            width = DefaultRoomData.DEFAULT_WIDTH;
            height = DefaultRoomData.DEFAULT_HEIGHT;
            transX = new Vector3(width / 2, 0, 0);
            transY = new Vector3(0, 0, height / 2);
        }

        public void DrawBounds(Vector3 position)
        {
            //Draw frame. 
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(width, -10, height));

            //Draw the edge points.
            Gizmos.color = new Color(1, 0, 0, 1f);
            Gizmos.DrawSphere(position + new Vector3(MinX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(MaxX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MinZ), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MaxZ), 0.5f);
        }
    }

    /// <summary>
    /// Generates a custom class for handling the target of the camera. 
    /// </summary>
    [System.Serializable]
    public struct TargetHandler
    {
        public Transform target;

        public readonly Vector3 Position => target.position;
        public readonly Vector3 XDecomposed => new Vector3(Position.x, 0, 0);
        public readonly Vector3 ZDecomposed => new Vector3(0, 0, Position.z);

        public bool InsideXAxis(Vector3 position, CameraBounds bounds)
        {
            return Position.x > position.x + bounds.MinX && Position.x < position.x + bounds.MaxX;
        }

        public bool InsideZAxis(Vector3 position, CameraBounds bounds)
        {
            return Position.z > position.z + bounds.MinZ && Position.z < position.z + bounds.MaxZ;
        }
    }
}

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Used for storing the difference of two vectors. 
    /// </summary>
    Vector3 _differenceVec;

    /// <summary>
    /// Stores the distance between the player position and the camera position. 
    /// </summary>
    float _distance;

    /// <summary>
    /// Bounds class, used to define the AABB box of the camera. 
    /// </summary>
    public CameraBounds bounds;

    /// <summary>
    /// The target handler used to track the players position/handle AABB x/y tests. 
    /// </summary>
    public TargetHandler handler;

    /// <summary>
    /// The current behavioural state used by the camera. 
    /// </summary>
    public CameraBehaviour currentBehaviour = CameraBehaviour.TARGET_STEP;

    /// <summary>
    /// Is the camera currently stepping. 
    /// </summary>
    public bool isStepping = false;

    public Vector3 Position
    {
        set => transform.position = value;
    }

    public float jumpDistance;

    private Vector3 DifferenceVecX => 
        (handler.XDecomposed - new Vector3(transform.position.x, 0, 0)).normalized;

    private Vector3 DifferenceVecZ =>
    (handler.XDecomposed - new Vector3(0, 0, transform.position.z)).normalized;

    private void Start()
    {
        bounds = new CameraBounds(); 
    }

    public void Update()
    {
        switch (currentBehaviour)
        {
            case CameraBehaviour.TARGET_STEP:
                if (!isStepping)
                    CameraStepper();
                break;
            case CameraBehaviour.LOCK:
                break;
            case CameraBehaviour.TRANSVERSE:
                break;
        }
    }

    /// <summary>
    /// Calculates the direction to step, if a step is required. 
    /// </summary>
    private void CameraStepper()
    {
        //Cache player if not cached. 
        if (handler.target == null && GameManager.Instance.playerCurrent != null)
            handler.target = GameManager.Instance.playerCurrent.transform;

        if (handler.target != null)
        {
            if (!handler.InsideXAxis(transform.position, bounds))
            {
                //Get the direction vector. 
                _differenceVec = DifferenceVecX;
                _distance = Vector3.Distance(new Vector3(transform.position.x, 0, 0), handler.XDecomposed);
                float sign = Mathf.Sign(_differenceVec.x);

                //Determine direction on which the player has breached AABB.
                if (sign * _distance < bounds.MinX) //Breached left, move camera left. 
                    StartCoroutine(StepCamera(transform.position, -bounds.TranslationX));
                if (sign * _distance > bounds.MaxX) //Breached right, move camera right. 
                    StartCoroutine(StepCamera(transform.position, bounds.TranslationX));
            }

            if (!handler.InsideZAxis(transform.position, bounds))
            {
                //Get the direction vector. 
                _differenceVec = DifferenceVecZ;
                _distance = Vector3.Distance(new Vector3(0, 0, transform.position.z), handler.ZDecomposed);
                float sign = Mathf.Sign(_differenceVec.z);

                //Determine direction on which the player has breached AABB.
                if (sign * _distance < bounds.MinZ) //Breached up, move camera up. 
                    StartCoroutine(StepCamera(transform.position, -bounds.TranslationZ));
                if (sign * _distance > bounds.MaxZ) //Breached down, move camera down.
                    StartCoroutine(StepCamera(transform.position, bounds.TranslationZ));
            }
        }
    }

    /// <summary>
    /// Coroutine handling lerping between the two given points over time. 
    /// </summary>
    /// <param name="initial"> The initial position. </param>
    /// <param name="stepVector"> The translation to apply. </param>
    private IEnumerator StepCamera(Vector3 initial, Vector3 stepVector)
    {
        float transitionDuration = 1.0f;  // Time in seconds for the transition.
        float currentTime = 0;

        // Double the step vector
        Vector3 targetPosition = initial + (stepVector * 2);

        // Set the camera to stepping.
        isStepping = true;

        Vector3 roomward = handler.target.position - transform.position;
        roomward.y = 0;
        roomward.Normalize();

        handler.target.GetComponent<MovementSys>().UpdatePause = true;
        handler.target.position = handler.target.position + (roomward * jumpDistance);

        // While currentTime is less than the duration, keep updating the position.
        while (currentTime < transitionDuration)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / transitionDuration);  // Ensure t doesn't go beyond 1.
            Vector3 currentStep = Vector3.Lerp(initial, targetPosition, t);
            Position = currentStep;
            yield return new WaitForEndOfFrame();
        }

        // Snap to the exact target position at the end of the movement.
        Position = targetPosition;

        // Release control after stepping.
        handler.target.GetComponent<MovementSys>().UpdatePause = false;
        isStepping = false;
    }

    public void OnDrawGizmos()
    {
        bounds.DrawBounds(transform.position);
    }
}
