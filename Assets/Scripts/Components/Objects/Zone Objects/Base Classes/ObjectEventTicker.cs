using System.Collections;
using UnityEngine;

/// <summary>
/// Class used for ticking over a set period of time. 
/// </summary>
public class ObjectEventTicker : MonoBehaviour
{

    /// <summary>
    /// Is the object timer active. 
    /// </summary>
    [SerializeField] private bool _timerActive = false;

    /// <summary>
    /// Is the current tick complete? 
    /// </summary>
    [SerializeField] private bool _tickComplete = false;

    /// <summary>
    /// The rate at which the ticks should occur. 
    /// </summary>
    public float _tickRate = 1F;

    /// <summary>
    ///  Returns true if the timer is active. 
    /// </summary>
    public bool TimerActive => _timerActive;

    /// <summary>
    /// Returns true if the current tick is elapsed. 
    /// </summary>
    public bool TickComplete
    {
        get => _tickComplete;
        set {
            //Check to see if the timer is already complete and if the value is false. 
            if (_tickComplete && !value)
                _tickComplete = value; //If conditions are met, set value.
        }
    }

    public bool Enable
    {
        //Simple return of the internal state.
        get => _timerActive;

        set
        {
            //Slightly complicated check to see
            //if the passed in value doesn't equal the cached value.
            if(value == _timerActive) return;
            if (value && !_timerActive)
                StartCoroutine(TickCooldown());
            else if (!value && _timerActive)
                StopCoroutine(TickCooldown());

            //Set new values. 
            _tickComplete = false;
            _timerActive = value;
        }
    }

    private void Update()
    {
        //If the tick is not complete and the timer is active, start tick. 
        if (_timerActive && !_tickComplete)
            StartCoroutine(TickCooldown());
    }

    private IEnumerator TickCooldown()
    {
        //Wait for the time to elapse. 
        yield return new WaitForSeconds(_tickRate);

        //Set the tick to complete. 
        _tickComplete = true;
    }
}
