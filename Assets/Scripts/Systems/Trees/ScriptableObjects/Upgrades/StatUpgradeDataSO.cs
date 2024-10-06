using UnityEngine;

[CreateAssetMenu(fileName = "New StatChangeItemDataSO", menuName = "Purrcifer/Collectable SO/New StatChangeItemDataSO")]
public class StatUpgradeDataSO : ItemDataSO
{
    
    public virtual void Apply() => GameManager.Instance.ApplyStatUpgrade = this;
}
