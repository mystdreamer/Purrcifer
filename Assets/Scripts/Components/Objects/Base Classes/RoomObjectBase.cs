using Purrcifer.Data.Defaults;
using UnityEngine;

public interface IRoomObject
{
    void AwakenObject();
    void SleepObject();
    void SetObjectState(WorldStateEnum state);
}

public enum ObjectActivationType
{
    ON_ROOM_ACTIVATION, 
    ON_OBJECT_START
}

/// <summary>
/// Class used to inherit needed behaviours for room object updating. 
/// </summary>
public abstract class RoomObjectBase : MonoBehaviour, IRoomObject
{
    [Header("What activation method the object follows.")]
    public ObjectActivationType activationType;

    /// <summary>
    /// Is the object currently active.
    /// </summary>
    private bool _objectActive = false;

    /// <summary>
    /// Has this object completed.
    /// </summary>
    private bool _objectCompleted = false;
    
    /// <summary>
    /// Represents whether the object has completed or not. 
    /// </summary>
    public bool ObjectComplete
    {
        get => _objectCompleted;
        internal set => _objectCompleted = value;
    }

    /// <summary>
    /// Returns whether the room object is interactable. 
    /// </summary>
    public bool ObjectActive => 
        (activationType == ObjectActivationType.ON_ROOM_ACTIVATION && _objectActive) | 
        (activationType != ObjectActivationType.ON_ROOM_ACTIVATION);

    /// <summary>
    /// Returns the name of the object. 
    /// </summary>
    /// <returns></returns>
    public string GetName() => gameObject.name;

    /// <summary>
    /// Sets all objects inside the room to active. 
    /// </summary>
    void IRoomObject.AwakenObject()
    {
        _objectActive = true;
        OnAwakeObject();
    }

    /// <summary>
    /// Sets all objects inside the room to inactive. 
    /// </summary>
    void IRoomObject.SleepObject()
    {
        _objectActive = false;
        OnSleepObject();
    }

    /// <summary>
    /// Sets the world state to the object. 
    /// </summary>
    /// <param name="state"> The state to set to the object. </param>
    void IRoomObject.SetObjectState(WorldStateEnum state) => SetWorldState(state);

    /// <summary>
    /// Implement object awakening logic here. 
    /// </summary>
    internal abstract void OnAwakeObject();

    /// <summary>
    /// Implement Object sleep logic here. 
    /// </summary>
    internal abstract void OnSleepObject();

    /// <summary>
    /// Sets the world state to the object. 
    /// </summary>
    /// <param name="state"> The state to set to the object. </param>
    internal abstract void SetWorldState(WorldStateEnum state);
}
