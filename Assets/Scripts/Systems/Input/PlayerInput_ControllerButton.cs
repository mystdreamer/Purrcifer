using UnityEngine;

namespace Purrcifer.Inputs.Container
{
    [System.Serializable]
    public class PlayerInput_ControllerButton : PlayerInput
    {
        /// <summary>
        /// Delegate used by this action. 
        /// </summary>
        /// <param name="result"> Bool representing if the input is active. </param>
        public delegate void MovementAction(bool result);

        /// <summary>
        /// The event to be subscribed to for input notifications. 
        /// </summary>
        public event MovementAction DoAction;

        /// <summary>
        /// The controller button assigned to the action. 
        /// </summary>
        public KeyCode Button;

        public PlayerInput_ControllerButton(KeyCode button, PlayerActionIdentifier identifier, string name)
        {
            this.Button = button;
            this.type = identifier;
            this.actionName = name;
        }

        public override void Command()
        {
            if (Input.GetKey(Button))
                DoAction?.Invoke(true);
            if (Input.GetKeyUp(Button))
                DoAction?.Invoke(false);
        }

        public override void Clear()
        {
            MovementAction.RemoveAll(DoAction, DoAction);
        }
    }
}