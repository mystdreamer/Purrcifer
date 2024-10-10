using JetBrains.Annotations;
using Purrcifer.Data.Defaults;
using System;
using System.Collections;
using UnityEngine;

public class WorldLightController : WorldObject
{
    WorldState stateLast;
    WorldState stateCurrent;


    public Light worldLight;
    public Color colorNormal;
    public Color colorWitchingHour;
    public Color colorHell;

    void Awake()
    {
        stateLast = stateCurrent = WorldState.WORLD_START;
        worldLight.color = colorNormal;
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        //Set new color over small period of time.
        stateLast = stateCurrent; 
        worldLight.color = GetColorType(state);
    }

    private Color GetColorType(WorldState state)
    {
        switch (state)
        {
            case WorldState.WORLD_START:
                return colorNormal;
            case WorldState.WORLD_WITCHING:
                return colorWitchingHour;
            case WorldState.WORLD_HELL:
                return colorHell;
        }

        //Catch fail cases. 
        Debug.Log("World Light Controller: No color defined");
        Debug.Break();
        return colorNormal;
    }
}
