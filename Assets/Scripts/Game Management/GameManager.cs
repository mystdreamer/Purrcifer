using System;
using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using JetBrains.Annotations;
using Assets.Scripts.Types.FloorGeneration;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public ObjectMap map;
    public GameObject playerPrefab;
    public GameObject playerCurrent; 
    public GameObject playerCameraPrefab;
    public FloorGenerationHandler floorGenerationHandler;
    public static GameManager Instance => _instance;


    void Awake()
    {
        #region Singleton Setup. 
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        #endregion

    }

    void Update()
    {

    }

    public void SetCamera(Vector3 position)
    {
        Camera.main.transform.position = position;
    }

    public void SpawnPlayerInstance(Vector3 position)
    {
        playerCurrent = GameObject.Instantiate(playerPrefab);
        playerCurrent.transform.position = position;
    }

    public void GenerateRandomMap(FloorData data) => floorGenerationHandler.GenerateRandomMap(data);

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        for (int x = 0; x < plan.plan.GetLength(0); x++)
    //        {
    //            for (int y = 0; y < plan.plan.GetLength(1); y++)
    //            {
    //                if (plan.plan[x, y] == 1)
    //                {
    //                    Gizmos.color = Color.white;
    //                    Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
    //                }

    //                if (plan.plan[x, y] == 2)
    //                {
    //                    Gizmos.color = Color.green;
    //                    Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
    //                }

    //                if (plan.plan[x, y] == 3)
    //                {
    //                    Gizmos.color = Color.red;
    //                    Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
    //                }

    //                if (plan.plan[x, y] == 4)
    //                {
    //                    Gizmos.color = Color.yellow;
    //                    Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
    //                }
    //            }
    //        }
    //    }
    //}
}
