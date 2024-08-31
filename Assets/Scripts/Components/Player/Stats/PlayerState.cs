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
        DataCarrier.Instance.GetPlayerState(ref _health, ref _damage);
        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
    }

    private void Update()
    {
        if (_health == null)
            return;
        UIManager.Instance.PlayerHealthBar.UpdateHealthbar(_health.Health);
    }

    public void OnDestroy()
    {
        DataCarrier.Instance.UpdatePlayerState(this);
    }
}
