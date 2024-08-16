using System;
using UnityEngine;

public class MovementSys : MonoBehaviour
{
    //Cache for the last input direction. 
    Vector3 _lastInput = Vector2.zero;

    //Cache for current input directions. 
    Vector3 _input = Vector2.zero;

    //Reference to the rigidbody. 
    public Rigidbody body;

    /// <summary>
    /// The players movement speed. 
    /// </summary>
    public float speed = 100;

    void Start()
    {
        //Register the required commands. 
        RegisterCommands();
    }

    private void OnDisable()
    {
        //Remove command registrations. 
        DeregisterCommands();
    }

    private void OnDestroy()
    {
        //Remove command registrations.  
        DeregisterCommands();
    }

    #region Movement Command Functions. 

    private void RegisterCommands()
    {
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_LEFT).DoAction += MoveRight;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_RIGHT).DoAction += MoveLeft;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_UP).DoAction += MoveDown;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_DOWN).DoAction += MoveUp;
    }

    private void DeregisterCommands()
    {
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_LEFT).DoAction -= MoveRight;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_RIGHT).DoAction -= MoveLeft;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_UP).DoAction -= MoveDown;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_DOWN).DoAction -= MoveUp;
    }

    private void MoveRight(bool result)
    {
        if (result)
            _input += new Vector3(-1, 0, 0);
    }

    private void MoveLeft(bool result)
    {
        if (result)
            _input += new Vector3(1, 0, 0);
    }

    private void MoveUp(bool result)
    {
        Debug.Log("Input Right: " + result);
        if (result)
            _input += new Vector3(0, 0, -1);
    }

    private void MoveDown(bool result)
    {
        if (result)
            _input += new Vector3(0, 0, 1);
    }
    #endregion

    void Update()
    {
        //Apply the velocity. 
        _input.Normalize();
        body.linearVelocity = _input * speed;
        _lastInput = _input;
        _input = Vector3.zero;
    }
}
