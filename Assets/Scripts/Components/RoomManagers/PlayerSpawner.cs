using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object responsible for spawning the player within a room. 
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        //On starting this object spawn the player and reposition the camera. 
        GameManager.Instance.SpawnPlayerInstance(transform.position);
        GameManager.Instance.SetCamera(new Vector3(transform.position.x, 5, transform.position.z));
    }
}
