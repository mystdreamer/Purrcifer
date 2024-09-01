using CameraHelpers;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
    public struct CameraBounds
    {
        public float width;
        public float height;

        public readonly float MinX => -width / 2;
        public readonly float MaxX => width / 2;
        public readonly float MinZ => -height / 2;
        public readonly float MaxZ => height / 2;

        public void DrawBounds(Vector3 position)
        {
            //Draw frame. 
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(position, new Vector3(width, 0, height));

            //Draw the edge points.
            Gizmos.color = new Color(1, 0, 0, 0.5F);
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

    public GameObject cameraCutout;

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
        set
        {
            transform.position = value;
            cameraCutout.transform.position = new Vector3(value.x, 5.62F, value.z);
        }
    }

    public float jumpDistance;

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
                _differenceVec = handler.XDecomposed - new Vector3(transform.position.x, 0, 0);
                _distance = Vector3.Distance(new Vector3(transform.position.x, 0, 0), handler.XDecomposed);
                _differenceVec.Normalize();

                //Determine direction on which the player has breached AABB.
                if (Mathf.Sign(_differenceVec.x) * _distance < bounds.MaxX) //Breached left, move camera left. 
                    StartCoroutine(StepCamera(transform.position, new Vector3(-bounds.width, 0, 0)));
                else if (Mathf.Sign(_differenceVec.x) * _distance > bounds.MaxX) //Breached right, move camera right. 
                    StartCoroutine(StepCamera(transform.position, new Vector3(bounds.width, 0, 0)));
            }

            if (!handler.InsideZAxis(transform.position, bounds))
            {
                //Get the direction vector. 
                _differenceVec = handler.ZDecomposed - new Vector3(0, 0, transform.position.z);
                _distance = Vector3.Distance(new Vector3(0, 0, transform.position.z), handler.ZDecomposed);
                _differenceVec.Normalize();

                //Determine direction on which the player has breached AABB.
                if (Mathf.Sign(_differenceVec.z) * _distance < bounds.MaxZ) //Breached up, move camera up. 
                    StartCoroutine(StepCamera(transform.position, new Vector3(0, 0, -bounds.height)));
                if (Mathf.Sign(_differenceVec.z) * _distance > bounds.MaxZ) //Breached down, move camera down.
                    StartCoroutine(StepCamera(transform.position, new Vector3(0, 0, bounds.height)));
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
        int frameBuffer = 12;
        //Cache variable for time and step. 
        float currentTime = 0;
        Vector3 currentStep = Vector3.zero;

        //Set the camera to stepping. 
        isStepping = true;

        Vector3 roomward = handler.target.position - transform.position;
        roomward.y = 0;
        roomward.Normalize();

        handler.target.GetComponent<MovementSys>().UpdatePause = true;
        handler.target.position = handler.target.position + (roomward * jumpDistance);

        //While not reached, increment the time, update the step and apply. 
        while (currentStep != initial + stepVector)
        {
            frameBuffer--;
            if (frameBuffer <= 0)
            {
                handler.target.GetComponent<MovementSys>().UpdatePause = false;
            }

            currentTime += Time.deltaTime;
            currentStep = Vector3.Lerp(initial, initial + stepVector, currentTime);
            Position = currentStep;
            yield return new WaitForEndOfFrame();
        }

        //Return control to the camera. 
        isStepping = false;
    }

    public void OnDrawGizmos()
    {
        bounds.DrawBounds(transform.position);
    }
}
