using UnityEngine;

[CreateAssetMenu(fileName = "New StatChangeItemDataSO", menuName = "Purrcifer/Collectable SO/New StatChangeItemDataSO")]
public class StatUpgradeItemDataSO : ItemDataSO
{
    
    public virtual void Apply() => GameManager.Instance.PlayerState.ApplyPowerup(this);
}
