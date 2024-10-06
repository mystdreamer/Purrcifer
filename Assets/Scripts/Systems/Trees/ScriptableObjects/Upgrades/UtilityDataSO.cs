using UnityEngine;

[CreateAssetMenu(fileName = "New UtilityDataSO", menuName = "Purrcifer/Collectable SO/UtilityDataSO")]
public class UtilityDataSO : ItemDataSO
{

    //Note any added item types to UtilityType need to be added to PlayerState ApplyPowerup(type).

    public enum UtilityType
    {
        STOPWATCH = 0,
    }

    public UtilityType type;

    public void Apply() => GameManager.Instance.ApplyUtilityUpgrade = this;
}
