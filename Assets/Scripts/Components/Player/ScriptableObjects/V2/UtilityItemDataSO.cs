using UnityEngine;

[CreateAssetMenu(fileName = "New UtilityItemDataSO", menuName = "Purrcifer/Collectable SO/New UtilityItemDataSO")]
public class UtilityItemDataSO : ItemDataSO
{
    public UtilityType type;

    public void Apply() => GameManager.Instance.PlayerState.ApplyPowerup(this);
}
