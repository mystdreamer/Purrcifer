using UnityEngine;
using Purrcifer.Data.Defaults;
using Unity.VisualScripting;

/// <summary>
/// Base class used for defining zone objects. 
/// </summary>
public abstract class ZoneObject : RoomObjectBase
{
    [SerializeField] private bool isAwake = false;
    internal bool detectionInverted = false;

    internal bool InZone => (detectionInverted) ? !isAwake : isAwake;

    public bool IsActive => isAwake;

    public ObjectEventTicker Ticker { get; set; }

    private void Awake()
    {
        if (activationType == ObjectActivationType.ON_OBJECT_START)
        {
            isAwake = true;
            if (Ticker != null)
                Ticker.Enable = true;
        }
    }

    internal override void OnAwakeObject()
    {
        base.ObjectComplete = true;

        if (activationType == ObjectActivationType.ON_OBJECT_START)
            return;

        isAwake = true;

        if (Ticker != null)
            Ticker.Enable = true;
    }

    internal override void OnSleepObject() {
        base.ObjectComplete = true;

        if (activationType == ObjectActivationType.ON_OBJECT_START)
            return;
        isAwake = false;

        if (Ticker != null)
            Ticker.Enable = false;
    }

    internal void FixedUpdate()
    {
        UpdateObject();
    }

    internal abstract void UpdateObject();
}
