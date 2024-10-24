using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    private bool hasCollided = false; 

    public abstract bool HasEvent { get; }

    public abstract bool HasDialogue { get; }

    public abstract ItemDialogue ItemDialogue
    {
        get;
    }

    public abstract PlayerEventData EventData { get; }

    public abstract WeaponDataSO WeaponData { get; }

    public abstract UtilityDataSO UtilityData { get; }

    public abstract ConsumableDataSO ConsumableData { get; }

    public abstract StatUpgradeDataSO StatUpgradeItemData { get; }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided)
            return;
        if (other.tag == "Player")
        {
            hasCollided = true;
            //Display the items dialogue. 
            if(HasDialogue) UIManager.SetDialogue(ItemDialogue);
            //If there is an event assigned to this behaviour fire the event change. 
            GameManager.Instance.SetPlayerDataEvent(EventData);
            //Apply the effect of the item. 
            GameManager.Instance.ApplyPowerup = this;
            OnApplicationEvent(other.gameObject);
        }
    }

    /// <summary>
    /// Override to implement powerup logic. 
    /// </summary>
    /// <param name="player"> The player object. </param>
    public abstract void OnApplicationEvent(GameObject player);
}

public abstract class PowerupConsumable : Powerup
{
    public ConsumableDataSO consumableData;

    public override bool HasEvent => false;

    public override bool HasDialogue => false;

    public override ItemDialogue ItemDialogue => null;

    public override PlayerEventData EventData => null;

    public override WeaponDataSO WeaponData => null;

    public override UtilityDataSO UtilityData => null;

    public override StatUpgradeDataSO StatUpgradeItemData => null;

    public override ConsumableDataSO ConsumableData => consumableData;
}

public abstract class PowerupWeapon : Powerup
{
    public WeaponDataSO weaponData;

    public override bool HasEvent => weaponData.eventData.hasEvent;

    public override bool HasDialogue => true;

    public override ItemDialogue ItemDialogue => weaponData.itemDialogue;

    public override PlayerEventData EventData => weaponData.eventData;

    public override WeaponDataSO WeaponData => weaponData;

    public override UtilityDataSO UtilityData => null;

    public override StatUpgradeDataSO StatUpgradeItemData => null;

    public override ConsumableDataSO ConsumableData => null;
}

public abstract class PowerupUtility : Powerup
{
    public UtilityDataSO utilityData;

    public override bool HasEvent => utilityData.eventData.hasEvent;

    public override bool HasDialogue => true;

    public override ItemDialogue ItemDialogue => utilityData.itemDialogue;

    public override PlayerEventData EventData => utilityData.eventData;

    public override WeaponDataSO WeaponData => null;

    public override UtilityDataSO UtilityData => utilityData;

    public override StatUpgradeDataSO StatUpgradeItemData => null;

    public override ConsumableDataSO ConsumableData => null;
}

public abstract class PowerupStatup : Powerup
{
    public StatUpgradeDataSO statupSO;

    public override bool HasEvent => statupSO.eventData.hasEvent;

    public override bool HasDialogue => true;

    public override ItemDialogue ItemDialogue => statupSO.itemDialogue;

    public override PlayerEventData EventData => statupSO.eventData;

    public override WeaponDataSO WeaponData => null;

    public override UtilityDataSO UtilityData => null;

    public override ConsumableDataSO ConsumableData => null;

    public override StatUpgradeDataSO StatUpgradeItemData => statupSO;
}