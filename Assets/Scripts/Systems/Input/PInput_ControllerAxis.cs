using UnityEngine;

namespace Purrcifer.Inputs.Container
{
    public class PlayInput_ControllerAxis
    {
        public const string LEFT_STICK_H = "Horizontal";
        public const string LEFT_STICK_V = "Vertical";
        public const string RIGHT_STICK_H = "RS_H";
        public const string RIGHT_STICK_V = "RS_V";
        public float axisValueX;
        public float axisValueZ;

        public Vector3 Command(PInputIdentifier type)
        {
            axisValueX = 0;
            axisValueZ = 0;

            if (type == PInputIdentifier.AXIS_LEFT_STICK)
            {
                axisValueX = Input.GetAxis(LEFT_STICK_H);
                axisValueZ = Input.GetAxis(LEFT_STICK_V);
            }

            if (type == PInputIdentifier.AXIS_RIGHT_STICK)
            {
                axisValueX = Input.GetAxis(RIGHT_STICK_H);
                axisValueZ = -1 * Input.GetAxis(RIGHT_STICK_V);
            }

            if (type == PInputIdentifier.AXIS_DPAD)
            {
                axisValueX = Input.GetAxis("DPAD_H");
                axisValueZ = Input.GetAxis("DPAD_V");
            }

            float deadZone = 0.15f;
            Vector3 direction = new Vector3(axisValueX, 0, axisValueZ);
            Vector3 returnVal = Vector3.zero;

            if (axisValueX > deadZone || axisValueX < -deadZone)
            {
                returnVal.x = direction.x;
            }

            if (axisValueZ > deadZone || axisValueZ < -deadZone)
            {
                returnVal.z = direction.z;
            }

            return returnVal.normalized;
        }
    }

    [System.Serializable]
    public class PInput_ControllerAxis : PlayerInput
    {
        public const string LEFT_STICK_H = "Horizontal";
        public const string LEFT_STICK_V = "Vertical";
        public const string RIGHT_STICK_H = "RS_H";
        public const string RIGHT_STICK_V = "RS_V";

        public float axisValueX;
        public float axisValueZ;

        /// <summary>
        /// Delegate used by this action. 
        /// </summary>
        /// <param name="result"> Bool representing if the input is active. </param>
        public delegate void MovementAction(Vector3 result);

        /// <summary>
        /// The event to be subscribed to for input notifications. 
        /// </summary>
        public event MovementAction DoAction;

        public PInput_ControllerAxis(PInputIdentifier identifier, string name)
        {
            this.type = identifier;
            this.actionName = name;
        }

        public override void Command()
        {
            axisValueX = 0;
            axisValueZ = 0;

            if (type == PInputIdentifier.AXIS_LEFT_STICK)
            {
                axisValueX = Input.GetAxis(LEFT_STICK_H);
                axisValueZ = Input.GetAxis(LEFT_STICK_V);
            }

            if (type == PInputIdentifier.AXIS_RIGHT_STICK)
            {
                axisValueX = Input.GetAxis(RIGHT_STICK_H);
                axisValueZ = -1 * Input.GetAxis(RIGHT_STICK_V);
            }

            if (type == PInputIdentifier.AXIS_DPAD)
            {
                axisValueX = Input.GetAxis("DPAD_H");
                axisValueZ = Input.GetAxis("DPAD_V");
            }

            float deadZone = 0.15f;
            Vector3 direction = new Vector3(axisValueX, 0, axisValueZ);
            Vector3 returnVal = Vector3.zero;

            if (axisValueX > deadZone || axisValueX < -deadZone)
            {
                returnVal.x = direction.x;
            }

            if (axisValueZ > deadZone || axisValueZ < -deadZone)
            {
                returnVal.z = direction.z;
            }

            DoAction?.Invoke(returnVal.normalized);
        }

        public override void Clear()
        {
            MovementAction.RemoveAll(DoAction, DoAction);
        }
    }
}