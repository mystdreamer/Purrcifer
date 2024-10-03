using Purrcifer.Data.Defaults;
using System.Collections;
using UnityEngine;

public class WorldLightController : WorldObject
{
    public Light worldLight; 
    public Color colorNormal; 
    public Color colorWitchingHour;
    public Color colorHell;
    
    void Awake()
    {
        worldLight.color = colorNormal;
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        //Set new color over small period of time.
        switch (state)
        {
            case WorldState.WORLD_START:
                worldLight.color = colorNormal;
                break;
            case WorldState.WORLD_WITCHING:
                worldLight.color = colorWitchingHour;
                break;
            case WorldState.WORLD_HELL:
                worldLight.color = colorHell;
                break;
        }
    }
}
