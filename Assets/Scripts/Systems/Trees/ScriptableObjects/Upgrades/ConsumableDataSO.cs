using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableDataSO", menuName = "Purrcifer/Collectable SO/ConsumableSO")]
public class ConsumableDataSO : ScriptableObject
{
    [System.Serializable]
    public enum ConsumableEffectInt
    {
        ADD_HEALTH = 0,
        REMOVE_HEALTH = 1,
        ADD_TALISMAN = 2,
        ADD_CHARGE = 3,
    }

    [System.Serializable]
    public enum ConsumableEffectBool
    {
        FILL_HEALTH = 0,
    }

    [System.Serializable]
    public struct IntConsumableEffect
    {
        public ConsumableEffectInt effect;
        public int value;
    }

    [System.Serializable]
    public struct BoolConsumableEffect
    {
        public ConsumableEffectBool type;
    }

    [Header("Item Identifiers")]
    public string itemName = "Consumable item here.";

    [Header("Item Prefabs.")]
    public GameObject powerupPrefab;

    public IntConsumableEffect[] intEffects;
    public BoolConsumableEffect[] boolEffects;
}