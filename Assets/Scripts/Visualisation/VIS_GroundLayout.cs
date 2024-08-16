using JetBrains.Annotations;
using UnityEngine;

public class VIS_GroundLayout : MonoBehaviour
{
    public int sizeWidth = 1;
    public int sizeHeight = 1;
    public int tileWidth = 10;
    public int tileHeight = 15;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        float z = gameObject.transform.position.y;

        for (int i = 0; i < tileHeight; i++)
        {
            for (int j = 0; j < tileWidth; j++)
            {
                float scaleX = sizeWidth * i;
                float scaleZ = sizeHeight * j;
                Vector2 posPosition = new Vector3(x + scaleX, 0, z + scaleZ);
                Gizmos.DrawWireSphere(posPosition, 0.25F);
            }
        }
    }


#if UNITY_EDITOR_WIN


#endif

}
