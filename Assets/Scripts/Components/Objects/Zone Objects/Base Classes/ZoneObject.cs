using UnityEngine;
using Purrcifer.Data.Defaults;
using Unity.VisualScripting;

/// <summary>
/// Base class used for defining zone objects. 
/// </summary>
public abstract class ZoneObject : RoomObjectBase
{
    [SerializeField] private bool insideArea = false;
    internal bool detectionInverted = false;

    internal bool InZone => (detectionInverted) ? !insideArea : insideArea;

    public bool IsActive => (ObjectActive && InZone);

    //internal Vector3 GetVector =>
    //    new Vector3(
    //        area.width / DefaultRoomData.DEFAULT_WIDTH, 
    //        1, 
    //        area.height / DefaultRoomData.DEFAULT_WIDTH);

    internal virtual void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            insideArea = true;
            OnEnterZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            insideArea = false;
            OnExitZone();
        }
    }

    internal virtual void Update()
    {
    }

    internal override void OnAwakeObject() => ObjectComplete = true;

    internal override void OnSleepObject() { }

    internal abstract void OnEnterZone();

    internal abstract void OnExitZone();
}
