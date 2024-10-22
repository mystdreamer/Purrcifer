using Purrcifer.Data.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStartingStatsSO", menuName = "Purrcifer/Scriptable Objects/PlayerStartingStatsSO")]
public class PlayerStartingStatsSO : ScriptableObject
{
    public string characterName;
    public int characterID; 
    public int minHealth;
    public int maxHealth;
    public int currentHealth;
    public float baseDamage;
    public float damageMultiplier;
    public float criticalHitDamage;
    public float criticalHitChance;
    public float attackRate; 
    public float movementSpeed;
    public int utilityCharges;
    public int[] weaponIDs;
    public int[] utilityIDs;
}