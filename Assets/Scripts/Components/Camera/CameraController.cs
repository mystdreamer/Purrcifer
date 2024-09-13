using CameraHelpers;
using Purrcifer.Data.Defaults;
using System.Collections;
using Unity.VisualScripting;
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

    #region Camera Step Controller. 
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
        
        /// <summary>
        /// Target vector representing the camera. 
        /// </summary>
        private TargetVector _camera;

        /// <summary>
        /// Target vector representing the target transform. 
        /// </summary>
        private TargetVector _target;

        /// <summary>
        /// the standard width of a room. 
        /// </summary>
        private readonly int _roomWidth;

        /// <summary>
        /// the standard height of a room. 
        /// </summary>
        private readonly int _roomHeight;
        
        /// <summary>
        /// Is the camera currently stepping. 
        /// </summary>
        private bool _isStepping;

        /// <summary>
        /// The time between transitions.
        /// </summary>
        private readonly float _transitionTime;

        /// <summary>
        /// The distance the player should step during transitions.
        /// </summary>
        private readonly float _playerStepDistance;

        /// <summary>
        /// Returns whether the camera is currently stepping. 
        /// </summary>
        public bool IsStepping => _isStepping;

        #region Vector Properties.
        public float MinX => -DefaultRoomData.DEFAULT_WIDTH / 2;
        public float MaxX => DefaultRoomData.DEFAULT_WIDTH / 2;
        public float MinZ => -DefaultRoomData.DEFAULT_HEIGHT / 2;
        public float MaxZ => DefaultRoomData.DEFAULT_HEIGHT / 2;
        public Vector3 TranslationX => new Vector3(_roomWidth, 0, 0);
        public Vector3 TranslationZ => new Vector3(0, 0, _roomHeight);

        public bool InsideXAxis => InsideAxis(_target.Position.x, _camera.Position.x, MinX, MaxX);
        public bool InsideZAxis => InsideAxis(_target.Position.z, _camera.Position.z, MinZ, MaxZ);

        public Vector3 Direction
        {
            get
            {
                Vector3 temp = (_target.Position - _camera.Position);
                temp.y = 0;
                return temp.normalized;
            }
        }
        public Vector3 DirectionX => (_target.XDecomposed - _camera.XDecomposed).normalized;
        public Vector3 DirectionZ => (_target.ZDecomposed - _camera.ZDecomposed).normalized;
        public float TargetDistanceX => Vector3.Distance(_camera.XDecomposed, _target.XDecomposed);
        public float TargetDistanceZ => Vector3.Distance(_camera.ZDecomposed, _target.ZDecomposed);
        #endregion

        public CameraStepHandler(Transform camera, Transform target)
        {
            this._target = new TargetVector(target);
            this._camera = new TargetVector(camera);
            _roomWidth = DefaultRoomData.DEFAULT_WIDTH;
            _roomHeight = DefaultRoomData.DEFAULT_HEIGHT;
            _playerStepDistance = DefaultCameraData.PLAYER_STEP_DISTANCE;
            _transitionTime = DefaultCameraData.STEP_TRANSITION_TIME;
        }

        private bool InsideAxis(float target, float pos, float min, float max)
        {
            return target > pos + min && target < pos + max;
        }

        public Vector3 GetStep()
        {
            float sign;
            float _distance;
            Vector3 _differenceVec;

            if (!InsideXAxis)
            {
                //Get the direction vector. 
                _differenceVec = DirectionX;
                _distance = TargetDistanceX;
                sign = Mathf.Sign(_differenceVec.x);

                //Determine direction on which the player has breached AABB.
                if (sign * _distance < MinX || sign * _distance > MaxX) //Breached left, move camera left. 
                    return sign * TranslationX;
            }

            if (!InsideZAxis)
            {
                //Get the direction vector. 
                _differenceVec = DirectionZ;
                _distance = TargetDistanceZ;
                sign = Mathf.Sign(_differenceVec.z);
                //Determine direction on which the player has breached AABB.
                if (sign * _distance < MinZ || sign * _distance > MaxZ) //Breached up, move camera up. 
                    return sign * TranslationZ;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Coroutine handling lerping between the two given points over time. 
        /// </summary>
        /// <param name="initial"> The initial position. </param>
        /// <param name="stepVector"> The translation to apply. </param>
        public IEnumerator StepCamera(Vector3 stepVector)
        {
            float currentTime = 0;

            Vector3 initialPosition = _camera.Position;
            Vector3 targetPosition = initialPosition + (stepVector);

            // Set the camera to stepping.
            _isStepping = true;

            Vector3 directionVec = Direction;
            GameManager.PlayerMovementPaused = true;
            _target.Position += (directionVec * _playerStepDistance);

            // While currentTime is less than the duration, keep updating the position.
            while (currentTime < _transitionTime)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / _transitionTime);  // Ensure t doesn't go beyond 1.
                _camera.Position = Vector3.Lerp(initialPosition, targetPosition, t);
                yield return new WaitForEndOfFrame();
            }

            // Snap to the exact target position at the end of the movement.
            _camera.Position = targetPosition;

            // Release control after stepping.
            GameManager.PlayerMovementPaused = false;
            _isStepping = false;
        }

        #region Gizmos. 
        public void DrawBounds(Vector3 position)
        {
            //Draw frame. 
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(_roomWidth, -10, _roomHeight));

            //Draw the edge points.
            Gizmos.color = new Color(1, 0, 0, 1f);
            Gizmos.DrawSphere(position + new Vector3(MinX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(MaxX, 0, 0), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MinZ), 0.5f);
            Gizmos.DrawSphere(position + new Vector3(0, 0, MaxZ), 0.5f);
        }
        #endregion
    }
    #endregion
}

public class CameraController : MonoBehaviour
{
    public CameraStepHandler stepHandler;

    /// <summary>
    /// The current behavioural state used by the camera. 
    /// </summary>
    public CameraBehaviour currentBehaviour = CameraBehaviour.TARGET_STEP;

    public Vector3 Position
    {
        set => transform.position = value;
    }

    public void Update()
    {
        switch (currentBehaviour)
        {
            case CameraBehaviour.TARGET_STEP:
                    Update_CameraStep();
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
    private void Update_CameraStep()
    {
        //Cache player if not cached. 

        if (GameManager.Instance.PlayerExists && stepHandler == null)
            stepHandler = new CameraStepHandler(transform, GameManager.Instance.PlayerTransform);

        if (stepHandler != null && !stepHandler.IsStepping)
        {
            Vector3 stepVector = stepHandler.GetStep();
            if (stepVector != Vector3.zero) //Breached left, move camera left.
                StartCoroutine(stepHandler.StepCamera(stepVector));
        }
    }

    public void OnDrawGizmos()
    {
        if (stepHandler != null)
            stepHandler.DrawBounds(transform.position);
    }
}
