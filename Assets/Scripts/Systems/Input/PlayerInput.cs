namespace Purrcifer.Inputs.Container
{
    [System.Serializable]
    public abstract class PlayerInput
    {
        /// <summary>
        /// The name assigned to the action. 
        /// </summary>
        public string actionName;

        /// <summary>
        /// The action type assigned. 
        /// </summary>
        public PlayerActionIdentifier type;

        public bool IsInput(PlayerActionIdentifier actionIdentifier)
        {
            return (int)type == (int)actionIdentifier;
        }

        /// <summary>
        /// Function for providing input data to subscribers. 
        /// </summary>
        public abstract void Command();

        public abstract void Clear();
    }
}