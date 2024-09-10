using System.Collections;
using UnityEngine;

public class ObjectEventTicker : MonoBehaviour
{

    [SerializeField] private bool _timerActive = false;
    [SerializeField] private bool _tickComplete = false;
    [SerializeField] private float _tickRate = 1F;

    public bool TimerActive => _timerActive;

    public bool TickComplete => _tickComplete;

    public bool Enable
    {
        get => _timerActive;

        set
        {
            if (value && !_timerActive)
            {
                StartCoroutine(TickCooldown());
            }
            else if (!value && _timerActive)
            {
                StopCoroutine(TickCooldown());
            }

            //Set new values. 
            _tickComplete = false;
            _timerActive = value;
        }
    }

    private void Update()
    {
        if (_timerActive && !_tickComplete)
        {
            StartCoroutine(TickCooldown());
        }
    }

    private IEnumerator TickCooldown()
    {
        yield return new WaitForSeconds(_tickRate);
        _tickComplete = true;
    }
}
