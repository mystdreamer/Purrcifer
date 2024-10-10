using UnityEngine;

public abstract class TalismanDestructable : MonoBehaviour
{
    public abstract void Resolve();
}

public class WallDestructor : TalismanDestructable
{
    public GameObject parentObject; 

    public override void Resolve()
    {
        Debug.Log("Talisman detected");
        parentObject.SetActive(false);
    }
}
