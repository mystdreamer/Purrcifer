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

    public void Start()
    {
        UpdateSize();
    }

    public override void OnAwakeObject() { }

    public override void OnSleepObject() { }

    internal override void SetWorldState(WorldStateEnum state) { }

    private void Update()
    {
        UpdateSize();
    }

    public void OnDrawGizmos()
    {
        if (area.transform == null)
            return;

        area.OnDraw();
    }

    private void UpdateSize()
    {
        gameObject.transform.position = area.transform.position;
        gameObject.transform.localScale = new Vector3(area.width, 0, area.height);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Interactable && !ticking)
        {
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
