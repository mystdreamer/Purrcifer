using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTester_ArchSprial : MonoBehaviour
{
    public GameObject bulletPrefab;
    public PolarPeriod3D polarPeriod;
    public float amplitude;
    public float wrapping;
    public int divisions;

    public void OnDrawGizmos()
    {
        Vector3[] points = PolarCoordBuilder.ArchimedeanSpiral(amplitude, wrapping, divisions, polarPeriod);
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + points[i], 0.25f);
        }
    }
}