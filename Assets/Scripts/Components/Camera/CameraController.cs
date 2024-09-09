using CameraHelpers;
using Purrcifer.Data.Defaults;
using System.Collections;
using Unity.VisualScripting;
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

    public class CameraStepHandler
    {
        public struct TargetVector
        {
            Transform target;
            public readonly Vector3 Position
            {
                get => target.position;
                set => target.position = value;
            }

            public readonly Vector3 XDecomposed => new Vector3(Position.x, 0, 0);
            public readonly Vector3 ZDecomposed => new Vector3(0, 0, Position.z);

            public TargetVector(Transform transform)
            {
                target = transform;
            }
        }

        private int _width;
        private int _height;
        public TargetVector target;
        public TargetVector camera;

        public float MinX => -DefaultRoomData.DEFAULT_WIDTH / 2;
        public float MaxX => DefaultRoomData.DEFAULT_WIDTH / 2;
        public float MinZ => -DefaultRoomData.DEFAULT_HEIGHT / 2;
        public float MaxZ => DefaultRoomData.DEFAULT_HEIGHT / 2;
        public Vector3 TranslationX => new Vector3(_width, 0, 0);
        public Vector3 TranslationZ => new Vector3(0, 0, _height);

        public bool InsideXAxis => InsideAxis(target.Position.x, camera.Position.x, MinX, MaxX);
        public bool InsideZAxis => InsideAxis(target.Position.z, camera.Position.z, MinZ, MaxZ);

        public Vector3 DifferenceVecX => (target.XDecomposed - camera.XDecomposed).normalized;
        public Vector3 DifferenceVecZ => (target.ZDecomposed - camera.ZDecomposed).normalized;
        public float TargetDistanceX => Vector3.Distance(camera.XDecomposed, target.XDecomposed);
        public float TargetDistanceZ => Vector3.Distance(camera.ZDecomposed, target.ZDecomposed);

        public CameraStepHandler(Transform camera, Transform target)
        {
            this.target = new TargetVector(target);
            this.camera = new TargetVector(camera);
            _width = DefaultRoomData.DEFAULT_WIDTH;
            _height = DefaultRoomData.DEFAULT_HEIGHT;
        }

        public bool InsideAxis(float target, float pos, float min, float max)
        {
            return target > pos + min && target < pos + max;
        }

        #region Gizmos. 
        public void DrawBounds(Vector3 position)
        {
            //Draw frame. 
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(_width, -10, _height));

            //Draw the edge points.
            Gizmos.color = new Color(1, 0, 0, 1f);
            Gizmos.DrawSphere(position + new Vector3(MinX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(MaxX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MinZ), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MaxZ), 0.5f);
        }
        #endregion
    }
}

public class CameraController : MonoBehaviour
{
    public CameraStepHandler stephandler;
    public Transform target;

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

        if (GameManager.Instance.playerCurrent != null && stephandler == null)
            stephandler = new CameraStepHandler(transform, GameManager.Instance.playerCurrent.transform);


        float sign;
        float _distance;
        Vector3 _differenceVec;
        if (stephandler != null && !isStepping)
        {
            if (!stephandler.InsideXAxis)
            {
                //Get the direction vector. 
                _differenceVec = stephandler.DifferenceVecX;
                _distance = stephandler.TargetDistanceX;
                sign = Mathf.Sign(_differenceVec.x);

                //Determine direction on which the player has breached AABB.
                if (sign * _distance < stephandler.MinX || sign * _distance > stephandler.MaxX) //Breached left, move camera left. 
                    StartCoroutine(StepCamera(transform.position, sign * stephandler.TranslationX));
            }

            if (!stephandler.InsideZAxis)
            {
                //Get the direction vector. 
                _differenceVec = stephandler.DifferenceVecZ;
                _distance = stephandler.TargetDistanceZ;
                sign = Mathf.Sign(_differenceVec.z);
                //Determine direction on which the player has breached AABB.
                if (sign * _distance < stephandler.MinZ || sign * _distance > stephandler.MaxZ) //Breached up, move camera up. 
                    StartCoroutine(StepCamera(transform.position, sign * stephandler.TranslationZ));
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
        float transitionDuration = 0.80F;  // Time in seconds for the transition.
        float currentTime = 0;

        Vector3 targetPosition = initial + (stepVector);

        // Set the camera to stepping.
        isStepping = true;

        Vector3 roomward = stephandler.target.Position - transform.position;
        roomward.y = 0;
        roomward.Normalize();

        GameManager.Instance.playerPrefab.GetComponent<MovementSys>().UpdatePause = true;
        stephandler.target.Position = stephandler.target.Position + (roomward * jumpDistance);

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
        GameManager.Instance.playerPrefab.GetComponent<MovementSys>().UpdatePause = false;
        isStepping = false;
    }

    public void OnDrawGizmos()
    {
        if (stephandler != null)
            stephandler.DrawBounds(transform.position);
    }
}
