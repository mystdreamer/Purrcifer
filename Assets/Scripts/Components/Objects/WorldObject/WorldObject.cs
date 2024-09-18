using Purrcifer.Data.Defaults;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    internal void Start()
    {
        GameManager.Instance.WorldStateChange += WorldUpdateReceiver;
    }

    internal void OnDisable()
    {
        GameManager.Instance.WorldStateChange -= WorldUpdateReceiver;
    }

    internal void OnDestroy()
    {
        GameManager.Instance.WorldStateChange -= WorldUpdateReceiver;
    }

    internal abstract void WorldUpdateReceiver(WorldState state);
}
