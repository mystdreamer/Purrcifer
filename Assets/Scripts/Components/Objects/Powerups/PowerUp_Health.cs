using UnityEngine;

public class PowerUp_Health : PowerupUtility
{
    public override void OnApplicationEvent(GameObject player)
    {
        //Do animation logic here. 

        //Cleanup object. 
        this.gameObject.SetActive(false);
    }
}