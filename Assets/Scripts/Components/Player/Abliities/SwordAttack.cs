using JetBrains.Annotations;
using UnityEngine;

public enum ItemType
{
    STOPWATCH = 0, 
    SWORD = 1,
}

public interface IAbilities
{
    public bool Execute(ItemType type);
}

public class SwordAttack : MonoBehaviour, IAbilities
{
    private ItemType _type = ItemType.SWORD;
    [SerializeField] private GameObject _swordPrefab;
    public int detectionPoints = 10;

    public bool Execute(ItemType type)
    {
        if (type == _type)
        {

        }
        return false;
    }

    private void CastPoints()
    {
        Vector3 position = gameObject.transform.forward;
    }

    public void OnDrawGizmos()
    {
        Vector3 position = gameObject.transform.position;
        Vector3 forward = gameObject.transform.forward;
        //Quaternion positiveRotation = Quaternion.Euler()
        forward.Normalize();
        Ray ray = new Ray(position, MovementSys.LastInput.normalized);
        Gizmos.DrawRay(ray);
    }
}
