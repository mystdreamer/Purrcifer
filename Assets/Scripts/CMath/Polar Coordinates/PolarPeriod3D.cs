using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a periodic arc of the unit circle. 
/// </summary>
[System.Serializable]
public struct PolarPeriod3D
{
    /// <summary>
    /// The minimum point of the arc. 
    /// </summary>
    public float minR;

    /// <summary>
    /// The maximum point of the arc. 
    /// </summary>
    public float maxR;

    /// <summary>
    /// The minimum point of the arc in degrees. 
    /// </summary>
    public float MinDegrees => minR * Mathf.Rad2Deg;

    /// <summary>
    /// The maximum point of the arc in degrees. 
    /// </summary>
    public float MaxDegrees => maxR * Mathf.Rad2Deg; 

    /// <summary>
    /// The minimum point of the arc in radians. 
    /// </summary>
    public float MinRadians => minR;

    /// <summary>
    /// The maximum point of the arc in radians. 
    /// </summary>
    public float MaxRadians => maxR;

    /// <summary>
    /// The radian length between the min and max point. 
    /// </summary>
    public float Length => (maxR - minR);

    /// <summary>
    /// CTOR: 
    /// </summary>
    /// <param name="min"> The minimum point of the arc in degrees. </param>
    /// <param name="max"> The maximum point of the arc in degrees. </param>
    public PolarPeriod3D(float min, float max)
    {
        minR = min * Mathf.Deg2Rad;
        maxR = max * Mathf.Deg2Rad;
    }
}
