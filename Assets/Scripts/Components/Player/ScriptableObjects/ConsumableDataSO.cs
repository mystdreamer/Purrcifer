using UnityEngine;



[CreateAssetMenu(fileName = "ConsumableDataSO", menuName = "Purrcifer/Collectable SO/ConsumableSO")]
public class ConsumableDataSO : ScriptableObject
{
    public enum ConsumableEffectInt
    {
        ADD_HEALTH = 0,
        REMOVE_HEALTH = 1,
        ADD_TALISMAN = 2,
        ADD_CHARGE = 3,
    }

    public enum ConsumableEffectBool
    {
        FILL_HEALTH = 0,
    }

    public struct IntConsumableEffect
    {
        public ConsumableEffectInt effect;
        public int value;
    }

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