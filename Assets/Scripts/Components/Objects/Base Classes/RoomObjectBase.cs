using Purrcifer.Data.Defaults;
using UnityEngine;

public interface IRoomObject
{
    void AwakenObject();
    void SleepObject();
}

public enum ObjectActivationType
{
    ON_ROOM_ACTIVATION, 
    ON_OBJECT_START
}

/// <summary>
/// Class used to inherit needed behaviours for room object updating. 
/// </summary>
public abstract class RoomObjectBase : WorldObject, IRoomObject
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
    /// Implement object awakening logic here. 
    /// </summary>
    internal abstract void OnAwakeObject();

    /// <summary>
    /// Implement Object sleep logic here. 
    /// </summary>
    internal abstract void OnSleepObject();
}

public abstract class RoomSubObject : WorldObject
{

}

public abstract class SubEntity : RoomSubObject
{
    [SerializeField] EntityHealth _health;

    public float CurrentHealth
    {
        get => _health.Health;
        set => _health.Health = value;
    }

    public float MaxHealth
    {
        get => _health.MaxCap;
        set => _health.MaxCap = value;
    }

    public void InitialiseHealth(int current, int min, int max)
    {
        _health = new EntityHealth(min, max, current);
    }

    public abstract void EnableSubObject();
    public abstract void DisableSubObject();
    internal abstract void SetWorldState(WorldState state);
    internal abstract void HealthChangedEvent(float lastValue, float currentValue);
    internal abstract void OnDeathEvent();
    internal abstract void InvincibilityActivated();
}