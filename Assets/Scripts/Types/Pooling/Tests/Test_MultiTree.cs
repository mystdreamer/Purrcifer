using JetBrains.Annotations;
using UnityEngine;

public class Test_MultiTree : MonoBehaviour
{
    public ItemMultiTree multiTree;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            multiTree.SpawnItem(new Vector2(1, 1), false);
        }
    }
}
