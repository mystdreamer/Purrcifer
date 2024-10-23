using UnityEngine;

[System.Serializable]
public struct Weapon_4DirectionalPrefabs
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public float destructionTime;
    public float cooldownTime;

    public GameObject ResolvePrefab(Vector3 direction)
    {
        if (direction == Vector3.right) return right;
        if (direction == Vector3.left) return left;
        if (direction == Vector3.forward) return up;
        if (direction == Vector3.back) return down;
        else return null;
    }
}
