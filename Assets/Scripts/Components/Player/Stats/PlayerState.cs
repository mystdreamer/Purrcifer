using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Class responsible for carrying player data statistics. 
/// </summary>
public class PlayerState : MonoBehaviour
{
    private const int IFRAMES = 35;
    private const int MAX_HEALTH_CAP_LIMIT = 12;
    [SerializeField] private PlayerHealthRange _health;
    [SerializeField] private PlayerDamageData _damage;
    [SerializeField] private bool invincible = false;

    public PlayerDamageData Damage => _damage;

    #region Health Properties. 
    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public int Health
    {
        get => _health.current;

        set
        {
            if (invincible)
                return;

            int initial = _health.current;
            _health.current = value;
            _health.current = math.clamp(_health.current, _health.min, _health.max);

            if (_health.current < initial)
            {
                //Player has taken damage. 
                StartCoroutine(DamageIframes());
                //--> start IFrames. 
            }
        }
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public int HealthMaxCap
    {
        get => _health.max;

        set
        {
            _health.max = value;

            //If the value is greater than the allowed max health cap,
            //reset it to the max.
            if (MAX_HEALTH_CAP_LIMIT < _health.max)
                _health.max = MAX_HEALTH_CAP_LIMIT;
        }
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public int HealthMinCap
    {
        get => _health.min;
        set => _health.min = value;
    }

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => _health.current == _health.min;

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public int Length => HealthMaxCap - HealthMinCap;
    #endregion

    private void Start()
    {
        DataCarrier.Instance.GetPlayerState(ref _health, ref _damage);
        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
    }

    private void Update()
    {
        if (_health == null)
            return;
        UIManager.Instance.PlayerHealthBar.UpdateHealthbar(_health.current);

        if (!Alive)
            GameManager.Instance.PlayerDeath();
    }

    public void OnDestroy()
    {
        DataCarrier.Instance.UpdatePlayerState(this);
    }

    private IEnumerator DamageIframes()
    {
        int iframes = IFRAMES;
        invincible = true;
        while (0 < iframes)
        {
            //--> Display damage effect. 
            yield return new WaitForEndOfFrame();
            iframes--;
        }

        invincible = false;
    }
}
