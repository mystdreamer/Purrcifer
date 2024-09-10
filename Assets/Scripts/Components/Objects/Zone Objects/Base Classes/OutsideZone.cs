using System.Collections;
using UnityEngine;

public abstract class OutsideZone : ZoneObject
{
    private bool IsInside => IsInside;

    public bool IsOutside => !InZone;
}

