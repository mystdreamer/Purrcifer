using Purrcifer.Data.Defaults;
using System;
using UnityEngine;

public class OnRoomCompletionSpawnItems : RoomObjectBase
{
    private WorldState _state;
    public GameObject[] objectsNormal;
    public GameObject[] objectsWitching;
    public GameObject[] objectsHell;

    internal override void OnAwakeObject() { }

    internal override void OnSleepObject() => SpawnItems();

    private void SpawnItems()
    {
        switch (_state)
        {
            case WorldState.WORLD_START:
                EnableArray(objectsNormal);
                break;
            case WorldState.WORLD_WITCHING:
                EnableArray(objectsWitching);
                break;
            case WorldState.WORLD_HELL:
                EnableArray(objectsHell);
                break;
            default:
                break;
        }
    }

    private void EnableArray(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        _state = state;
    }
}
