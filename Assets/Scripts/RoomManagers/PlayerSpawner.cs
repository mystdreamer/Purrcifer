using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SpawnPlayerInstance(transform.position);
        GameManager.Instance.SetCamera(new Vector3(transform.position.x, 5, transform.position.z));
    }
}
