using System.Collections;
using UnityEngine;

public abstract class ZoneObject : RoomObjectBase
{
    public ObjectTickDamage damage;
    public AreaBounds area;
    public bool ticking = false;
    public bool insideArea = false;
    public float cooldownPeriod = 1f;
    public Vector3 lastSize;
    public Transform roomParent;

    internal Vector3 GetVector => new Vector3(area.width / 36, 1, area.height / 20);

    public void Start()
    {
        UpdateSize();
    }

    internal override void OnAwakeObject() => ObjectComplete = true;

    internal override void OnSleepObject() { }

    public void OnDrawGizmos()
    {
        if (area.transform == null)
            return;
        UpdateSize();
        area.OnDraw();
    }

    internal void UpdateSize()
    {
        if (lastSize != GetVector)
        {
            lastSize = GetVector;
            gameObject.transform.position = area.transform.position;
            gameObject.transform.localScale = GetVector;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            insideArea = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            insideArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            insideArea = true;
    }

    internal IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownPeriod);
        ticking = false;
    }
}
