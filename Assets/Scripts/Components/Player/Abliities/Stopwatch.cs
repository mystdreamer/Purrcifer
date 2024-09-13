using System.Collections;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    private bool _delayActive = false;
    public Range charge = new Range() { min = 0, max = 5, current = 5 };
    [SerializeField] float rateOfChange = 1f;
    [SerializeField] float _delayRate = 0.25F;

    public float Charge
    {
        get => charge.current;
        set => charge.current = value;
    }

    public bool CanUseStopwatch => (Charge > charge.min) && !_delayActive;

    public bool ModificationsActive => GameManager.WorldClock.AdditionOpActive | GameManager.WorldClock.RemoveOpActive;

    public int GetInputKeyboard
    {
        get
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log("Key Pressed Q: Reducing time.");
                return -1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Key Pressed E: Adding time.");
                return 1;
            }
            return 0;
        }
    }

    public void Update()
    { 
        if (CanUseStopwatch)
        {
            float modificationValue = rateOfChange * GetInputKeyboard;

            if (!ModificationsActive && modificationValue != 0)
            {
                Debug.Log("Stopwatch: Modifying time.");

                if(modificationValue < 0)
                    GameManager.WorldClock.RemoveValue(modificationValue);
                if(modificationValue > 0)
                    GameManager.WorldClock.AddValue(modificationValue);
                Charge -= 1;
                StartCoroutine(StopwatchDelay());
            }
        }
    }

    private IEnumerator StopwatchDelay()
    {
        _delayActive = true;

        yield return new WaitForSeconds(_delayRate);

        _delayActive = false;
    }
}