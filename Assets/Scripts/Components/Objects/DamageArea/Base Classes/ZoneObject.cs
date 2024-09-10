using UnityEngine;
using Purrcifer.Data.Defaults;

public abstract class ZoneObject : RoomObjectBase
{
    [SerializeField] private bool insideArea = false;
    public AreaBounds area;
    public Vector3 lastSize;
    public Transform roomParent;

    internal bool InZone => insideArea;

    internal Vector3 GetVector =>
        new Vector3(area.width / DefaultRoomData.DEFAULT_WIDTH, 1, area.height / DefaultRoomData.DEFAULT_WIDTH);

    internal virtual void Start()
    {
        UpdateSize();
    }

    public void OnDrawGizmos()
    {
        if (area.transform == null)
            return;
        UpdateSize();
        area.OnDraw();
    }

    #region Size Updating. 
    internal void UpdateSize()
    {
        if (lastSize != GetVector)
        {
            lastSize = GetVector;
            gameObject.transform.position = area.transform.position;
            gameObject.transform.localScale = GetVector;
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            insideArea = true;
            OnEnterZone();
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //        OnEnterZone();
    //}

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
        UpdateSize();
    }

    internal override void OnAwakeObject() => ObjectComplete = true;

    internal override void OnSleepObject() { }

    internal abstract void OnEnterZone();

    internal abstract void OnExitZone();
}
