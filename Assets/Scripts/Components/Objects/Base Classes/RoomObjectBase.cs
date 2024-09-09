using Purrcifer.Data.Defaults;
using UnityEngine;
/// <summary>
/// Class used to inherit needed behaviours for room object updating. 
/// </summary>
public abstract class RoomObjectBase : MonoBehaviour
{
    [SerializeField] private bool interactable = false;
    [SerializeField] private bool completed = false;

    /// <summary>
    /// Represents whether the object has completed or not. 
    /// </summary>
    public bool Complete
    {
        get => completed;
        internal set => completed = value;
    }

    /// <summary>
    /// Returns whether the room object is interactable. 
    /// </summary>
    public bool Interactable { get => interactable; }

    /// <summary>
    /// Sets all objects inside the room to active. 
    /// </summary>
    public void AwakenRoom()
    {
        interactable = true;
        OnAwakeObject();
    }

    /// <summary>
    /// Sets all objects inside the room to inactive. 
    /// </summary>
    public void SleepRoom()
    {
        interactable = false;
        OnSleepObject();
    }

    /// <summary>
    /// Implement object awakening logic here. 
    /// </summary>
    public abstract void OnAwakeObject();

    /// <summary>
    /// Implement Object sleep logic here. 
    /// </summary>
    public abstract void OnSleepObject();

    /// <summary>
    /// Sets the world state to the object. 
    /// </summary>
    /// <param name="state"> The state to set to the object. </param>
    internal abstract void SetWorldState(WorldStateEnum state);

    /// <summary>
    /// Returns the name of the object. 
    /// </summary>
    /// <returns></returns>
    public string GetName() => gameObject.name;
}
