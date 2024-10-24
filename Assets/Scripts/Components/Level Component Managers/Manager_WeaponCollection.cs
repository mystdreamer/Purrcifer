using DataManager;
using UnityEngine;

public class Manager_WeaponCollection : MonoBehaviour
{
    public GameObject hammerObject;
    public GameObject knifeObject;
    public GameObject sacDaggerObject;
    public GameObject spearObject;

    private void Start()
    {
        hammerObject.SetActive(DataCarrier.GetEventState(3001));
        knifeObject.SetActive(DataCarrier.GetEventState(3002));
        sacDaggerObject.SetActive(DataCarrier.GetEventState(3003));
        spearObject.SetActive(DataCarrier.GetEventState(3003));
    }

}
