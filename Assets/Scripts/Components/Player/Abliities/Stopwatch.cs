using System.Collections;
using UnityEngine;

/// <summary>
/// Class handling updating logic for the stopwatch. 
/// </summary>
public class Stopwatch : MonoBehaviour
{
    /// <summary>
    /// Returns whether the delay is currently active. 
    /// </summary>
    private bool _delayActive = false;

    /// <summary>
    /// The range class that is used to define the limits of usage for the timer. 
    /// </summary>
    public Range charge = new Range(5, 0, 5);

    /// <summary>
    /// The value at which the stopwatch changes time on press. 
    /// </summary>
    [SerializeField] float rateOfChange = 1f;

    /// <summary>
    /// The period of time the stopwatch should be delayed per activation.
    /// </summary>
    [SerializeField] float _delayRate = 0.25F;

    /// <summary>
    /// The current usable charges available. 
    /// </summary>
    public float UsableCharge
    {
        get => charge.current;
        set => charge.current = value;
    }

    /// <summary>
    /// Can the stopwatch be activated. 
    /// </summary>
    public bool CanUseStopwatch => (UsableCharge > charge.min) && !_delayActive;

    /// <summary>
    /// Returns required input data as an integer value. 
    /// </summary>
    public int GetInput
    {
        get
        {
            if (Input.GetKey(KeyCode.Q) | Input.GetKey(KeyCode.Joystick1Button4))
            {
                Debug.Log("Key Pressed Q: Reducing time.");
                return -1;
            }
            if (Input.GetKey(KeyCode.E) | Input.GetKey(KeyCode.Joystick1Button5))
            {
                Debug.Log("Key Pressed E: Adding time.");
                return 1;
            }
            return 0;
        }
    }

    public void Update()
    {
        if (!CanUseStopwatch)
            return;

        float input = rateOfChange * GetInput;

        if (input != 0)
        {
            Debug.Log("Stopwatch: Modifying time.");

            if (input < 0)
                GameManager.WorldClock.RemoveValue(input);
            if (input > 0)
                GameManager.WorldClock.AddValue(input);
            UsableCharge -= 1;
            StartCoroutine(StopwatchDelay());
        }
    }

    private IEnumerator StopwatchDelay()
    {
        _delayActive = true;
        yield return new WaitForSeconds(_delayRate);
        _delayActive = false;
    }
}