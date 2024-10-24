using DataManager;
using UnityEngine;

public class Manager_PlayerClasses : MonoBehaviour
{
    public GameObject tankObject;
    public GameObject glassCannonObject;

    public void OnEnable()
    {
        tankObject.SetActive(DataCarrier.GetEventState(2002));
        glassCannonObject.SetActive(DataCarrier.GetEventState(2001));
    }
}
