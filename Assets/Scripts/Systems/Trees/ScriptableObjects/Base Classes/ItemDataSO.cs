using UnityEngine;

public abstract class ItemDataSO : ScriptableObject
{
    //Note any added value types need to be addressed in the PlayerState ApplyValueTypes(type)
    public enum IntItemValueType : int
    {
        HEALTH_CAP = 1,
        PLAYER_UTILITY_CHARGE = 2,
    }

    public enum BoolItemValueType : int
    {
        FILL_HEALTH = 0,
        REDUCE_HEALTH_TO_ONE = 1,
    }

    public enum FloatItemValueType : int
    {
        MOVEMENT_SPEED = 1,
        DAMAGE_BASE = 3,
        DAMAGE_MULTIPLIER = 4,
        DAMAGE_CRITICAL_HIT = 5,
        DAMAGE_CRITICAL_CHANCE = 6,
    }

    [System.Serializable]
    public class StatChangeEffect
    {
        public BoolItemValueType type;
    }

    [System.Serializable]
    public class StatChangeFloat
    {
        public FloatItemValueType type;
        public float value;
    }

    [System.Serializable]
    public class StatChangeInt
    {
        public IntItemValueType type;
        public int value;
    }


    [Header("Item Identifiers")]
    [Header("|    The key identifier of the item.")]
    public int itemID = RandomIdGenerator.GetBase62(5);
    [Header("|    The weighted probability of the item.")]
    [Range(0, 100)]
    public int itemWeight; 
    [Header("|    Event data tied to this item.")]
    public PlayerEventData eventData;
    [Header("|    Dialogue data tied to this item.")]
    public ItemDialogue itemDialogue;

    [Header("|    The prefab to spawn on the pedistool.")]
    public GameObject powerupPedistoolPrefab;

    [Header("Stat change data.")]
    public StatChangeInt[] statChangeInts;
    public StatChangeFloat[] statChangeFloats;
    public StatChangeEffect[] statChangeEffects;
}
