using UnityEngine;

public class PowerUp_Health : PowerupStatup
{
    public override void OnApplicationEvent(GameObject player)
    {
        //Do animation logic here. 

        //Cleanup object. 
        this.gameObject.SetActive(false);
    }
}
