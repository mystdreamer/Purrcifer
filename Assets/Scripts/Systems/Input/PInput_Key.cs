using UnityEngine;
using UnityEngine.InputSystem;

namespace Purrcifer.Inputs.Container
{
    /// <summary>
    /// Class handling data for player inputs. 
    /// </summary>
    [System.Serializable]
    public class PInput_Key : PlayerInput
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

        public PInput_Key(KeyCode key, PInputIdentifier identifier, string name)
        {
            this.actionName = name;
            this.type = identifier;
            this.key = key;
        }

        public override void Command() => DoAction?.Invoke(Input.GetKey(key));

        public override void Clear() => MovementAction.RemoveAll(DoAction, DoAction);
    }
}