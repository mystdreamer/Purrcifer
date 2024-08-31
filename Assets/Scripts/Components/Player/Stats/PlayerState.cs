using System;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Class responsible for carrying player data statistics. 
/// </summary>
public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerHealthRange _health;
    [SerializeField] private PlayerDamageData _damage;

    public PlayerHealthRange Health => _health; 
    public PlayerDamageData Damage => _damage;

    private void Start()
    {
#if UNITY_EDITOR_WIN
        _health = PlayerHealthRange.GetTestDefault();
        _damage = PlayerDamageData.GetTestDefault();
#endif
    }
}
