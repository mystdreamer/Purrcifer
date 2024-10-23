using UnityEngine;

public class SacDagger : OrbitalWeapons
{
    public int damage;

    private float Damage
    {
        get
        {
            return damage * GameManager.Instance.PlayerState.DamageMultiplier;
        }
    }

    private void Update()
    {
        UpdateRotation();
    }

    public override void ApplyAttack(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        Boss boss = obj.GetComponent<Boss>();

        if (enemy != null) enemy.CurrentHealth -= Damage;
        if (boss != null) boss.Health -= Damage;
    }

    //Deathblossom is a girls girl with a giant moist flower. 
}
