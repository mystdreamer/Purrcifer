using UnityEngine;
using ItemPool;
using System.Collections.Generic;

public class BTREETester : MonoBehaviour
{
    public BBST<int> testBBST = new BBST<int>();

    void Start()
    {
        testBBST.Insert(new Node<int>(100, 1, 10));
        testBBST.Insert(new Node<int>(200, 2, 20));
        testBBST.Insert(new Node<int>(300, 3, 10));
        testBBST.Insert(new Node<int>(400, 4, 20));
    }

    void Update()
    {
        
    }
}
