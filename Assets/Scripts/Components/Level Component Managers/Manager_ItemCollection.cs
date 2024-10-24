using DataManager;
using UnityEngine;

public class Manager_ItemCollection : MonoBehaviour
{
    public GameObject pillsPrefab;
    public GameObject maskPrefab;
    public GameObject stopwatchPrefab;
    public GameObject bigBookOfPainPrefab;
    public GameObject littleBookOfPainPrefab;
    public GameObject burningHeartPrefab;
    public GameObject fleshPrefab;
    public GameObject sugarPrefab;
    public GameObject cookiePrefab;
    public GameObject chocolatePrefab;
    public GameObject normalHeartPrefab;       

    private void Start()
    {
        pillsPrefab.SetActive(DataCarrier.GetEventState(1003));
        maskPrefab.SetActive(DataCarrier.GetEventState(1004));
        stopwatchPrefab.SetActive(DataCarrier.GetEventState(1005));
        bigBookOfPainPrefab.SetActive(DataCarrier.GetEventState(1006));
        littleBookOfPainPrefab.SetActive(DataCarrier.GetEventState(1007));
        burningHeartPrefab.SetActive(DataCarrier.GetEventState(1008));
        fleshPrefab.SetActive(DataCarrier.GetEventState(1009));
        sugarPrefab.SetActive(DataCarrier.GetEventState(1010));
        cookiePrefab.SetActive(DataCarrier.GetEventState(1011));
        chocolatePrefab.SetActive(DataCarrier.GetEventState(1012));
        normalHeartPrefab.SetActive(DataCarrier.GetEventState(1013));
    }
}
