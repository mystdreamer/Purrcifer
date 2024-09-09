using UnityEngine;

[System.Serializable]
public struct AreaBounds
{
    public Transform transform;
    public float width;
    public float height;

    public Vector3 MinX => transform.position - new Vector3(width / 2, 0, 0);
    public Vector3 MaxX => transform.position + new Vector3(width / 2, 0, 0);

    public Vector3 MinZ => transform.position - new Vector3(0, 0, height / 2);
    public Vector3 MaxZ => transform.position + new Vector3(0, 0, height / 2);

    public void OnDraw()
    {
        Gizmos.color = Color.red - new Color(0, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(width, 0, height));
    }
}
