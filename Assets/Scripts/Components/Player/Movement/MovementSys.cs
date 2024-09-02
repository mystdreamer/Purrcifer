using UnityEngine;

/// <summary>
/// Movement behaviour controlling the main player character. 
/// </summary>
public class MovementSys : MonoBehaviour
{
    //Cache for the last input direction. 
    Vector3 _lastInput = Vector2.zero;

    //Cache for current input directions. 
    Vector3 _input = Vector2.zero;

    //Reference to the rigidbody. 
    public Rigidbody body;

    [SerializeField] private bool canUpdate = false;

    /// <summary>
    /// The players movement speed. 
    /// </summary>
    public float speed = 100;

    public Vector3 LastInput => _lastInput;
    public bool UpdatePause { 
        get => canUpdate; 
        set
        {
            Debug.Log(">>Player::MoveSys - State [" +  value + "].");
            canUpdate = value;
        } 
    } 

    void Start() 
    {
        canUpdate = false;
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

    // ---------------------------------
    // These act as registerable commands that interface with the custom input manager (PlayerInputSys). 
    // The input system is set up this way to allow for extra control with assigning/reassigning inputs. 
    // ---------------------------------

    /// <summary>
    /// Register the commands functions used by this class with the command events in the input system. 
    /// </summary>
    private void RegisterCommands()
    {
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_LEFT).DoAction += MoveRight;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_RIGHT).DoAction += MoveLeft;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_UP).DoAction += MoveDown;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_DOWN).DoAction += MoveUp;
    }

    /// <summary>
    /// Deregister the commands functions used by this class from the command events in the input system. 
    /// </summary>
    private void DeregisterCommands()
    {
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_LEFT).DoAction -= MoveRight;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_RIGHT).DoAction -= MoveLeft;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_UP).DoAction -= MoveDown;
        PlayerInputSys.Instance.GetAction(PlayerActionIdentifier.M_DOWN).DoAction -= MoveUp;
    }

    /// <summary>
    /// Function handling right movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveRight(bool state)
    {
        if (state)
            _input += new Vector3(-1, 0, 0);
    }

    /// <summary>
    /// Function handling left movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveLeft(bool result)
    {
        if (result)
            _input += new Vector3(1, 0, 0);
    }

    /// <summary>
    /// Function handling up movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveUp(bool result)
    {
        if (result)
            _input += new Vector3(0, 0, -1);
    }

    /// <summary>
    /// Function handling down movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveDown(bool result)
    {
        if (result)
            _input += new Vector3(0, 0, 1);
    }
    #endregion

    private void Update()
    {
        if (UpdatePause)
        {
            _input = _lastInput = Vector3.zero;
            body.linearVelocity = Vector3.zero;
            return;
        }
    }

    void LateUpdate()
    {
        if (UpdatePause)
            return;

        //Normalise the given input. 
        _input.Normalize();

        //Apply the given movement to the players rigidbody multiplied by the players speed. 
        body.linearVelocity = _input * speed;

        //Cache the last input (possible to be used for attacks).
        //TODO: Remove later if unused. 
        _lastInput = _input;

        //Reset the current input. 
        _input = Vector3.zero;
    }
}
