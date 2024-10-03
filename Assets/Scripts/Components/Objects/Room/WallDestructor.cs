using UnityEngine;

public class WallDestructor : MonoBehaviour, IOnTalisman
{
    public GameObject parentObject; 

    public void Resolve()
    {
        parentObject.SetActive(false);
    }
}
