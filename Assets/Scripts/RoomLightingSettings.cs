using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LevelLightingSettings", menuName = "Level/Lighting Settings")]
public class LevelLightingSettings : ScriptableObject
{
    public Material skybox;
    public float ambientIntensity = 1.0f;
    public Color ambientSkyColor = Color.white;
    public Color ambientEquatorColor = Color.white;
    public Color ambientGroundColor = Color.white;
    public DefaultReflectionMode defaultReflectionMode;
    public int defaultReflectionResolution = 128;
    public float reflectionIntensity = 1.0f;
    public bool fog;
    public Color fogColor = Color.grey;
    public FogMode fogMode = FogMode.Linear;
    public float fogDensity = 0.01f;
    public float fogStartDistance = 0;
    public float fogEndDistance = 300;
}