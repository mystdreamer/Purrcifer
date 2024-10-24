using DataManager;
using UnityEngine;

public class Manager_BossKills : MonoBehaviour
{
    public GameObject leechModel;
    public GameObject popwormModel;

    public void OnEnable()
    {
        popwormModel.SetActive(DataCarrier.GetEventState(1001));
        leechModel.SetActive(DataCarrier.GetEventState(1002));
    }
}
