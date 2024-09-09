using Purrcifer.Data.Defaults;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct ObjectTickDamage
{
    public int normalDamage;
    public int witchingDamage;
    public int hellDamage;
}

public class DamageArea : RoomObjectBase
{
    public ObjectTickDamage damage;
    public AreaBounds area;
    public bool ticking = false;
    public float cooldownPeriod = 1f;
    public Vector3 lastSize;

    public Transform roomParent;

    private Vector3 GetVector => new Vector3(area.width / 36, 1, area.height / 20);

    public void Start()
    {
        UpdateSize();
    }

    internal override void OnAwakeObject()
    {

        ObjectComplete = true;
    }

    internal override void OnSleepObject() { }

    internal override void SetWorldState(WorldStateEnum state) { }

    private void Update()
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

    private void UpdateSize()
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
            OnCollisionResolve();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            OnCollisionResolve();        
    }

    private void OnCollisionResolve()
    {
        if (ticking == false && ObjectUpdatable)
        {
            Debug.Log("Colllision Occured: Damage Area -> Player");
            ticking = true;
            GameManager.Instance.playerState.AddDamage = 1;
            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownPeriod);
        ticking = false;
    }
}
