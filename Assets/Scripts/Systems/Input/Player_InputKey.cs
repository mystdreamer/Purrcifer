using UnityEngine;
using UnityEngine.InputSystem;

namespace Purrcifer.Inputs.Container
{
    /// <summary>
    /// Class handling data for player inputs. 
    /// </summary>
    [System.Serializable]
    public class Player_InputKey : PlayerInput
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
        /// The key assigned to the action. 
        /// </summary>
        public KeyCode key;

        public Player_InputKey(KeyCode key, PlayerActionIdentifier identifier, string name)
        {
            this.actionName = name;
            this.type = identifier;
            this.key = key;
        }

        public override void Command()
        {
            if (Input.GetKey(key))
                DoAction?.Invoke(true);
            if (Input.GetKeyUp(key))
                DoAction?.Invoke(false);
        }

        public override void Clear()
        {
            MovementAction.RemoveAll(DoAction, DoAction);
        }
    }
}