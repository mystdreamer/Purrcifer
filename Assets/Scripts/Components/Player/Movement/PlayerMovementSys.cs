using Purrcifer.Data.Player;
using Purrcifer.Inputs.Container;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Movement behaviour controlling the main player character. 
/// </summary>
public class PlayerMovementSys : MonoBehaviour
{
    //Cache for the last input direction. 
    static Vector3 _lastInput = Vector2.zero;

    //Cache for current input directions. 
    Vector3 _input = Vector2.zero;

    //Reference to the rigidbody. 
    [SerializeField] private Rigidbody _body;
    private PlayerInputs _inputs;
    private PlayInput_ControllerAxis inputAxisResolver = new PlayInput_ControllerAxis();

    /// <summary>
    /// The players movement speed. 
    /// </summary>
    public float speed = 100;

    [SerializeField] private float rotationSpeed = 10f;

    public static Vector3 LastInput => _lastInput;

    public Vector3 ApplyForce
    {
        set => _body.AddForce(value, ForceMode.Force);
    }

    public Rigidbody RigidBody => _body;

    public static bool UpdatePause
    {
        get; set;
    } = false;

    void Start()
    {
        _inputs = GameManager.PlayerInputs;
    }

    #region Movement Command Functions. 

    /// <summary>
    /// Function handling right movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveRight(bool state)
    {
        if (state)
            _input += new Vector3(1, 0, 0);
    }

    /// <summary>
    /// Function handling left movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveLeft(bool result)
    {
        if (result)
            _input += new Vector3(-1, 0, 0);
    }

    /// <summary>
    /// Function handling up movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveUp(bool result)
    {
        if (result)
            _input += new Vector3(0, 0, 1);
    }

    /// <summary>
    /// Function handling down movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveDown(bool result)
    {
        if (result)
            _input += new Vector3(0, 0, -1);
    }

    /// <summary>
    /// Function handling down movement input from the input handler (PlayerInputSys).
    /// </summary>
    /// <param name="state"> The current input state provided. </param>
    private void MoveAxis(Vector3 result)
    {
        _input += result;
    }

    #endregion

    private void Update()
    {
        if (UpdatePause)
        {
            _input = _lastInput = Vector3.zero;
            _body.linearVelocity = Vector3.zero;
            return;
        }
        else
        {
            MoveAxis(inputAxisResolver.Command(_inputs.axis_m_left));

            if (Input.GetKey(_inputs.key_m_up))
                MoveUp(true);

            if (Input.GetKey(_inputs.key_m_down))
                MoveDown(true);

            if (Input.GetKey(_inputs.key_m_left))
                MoveLeft(true);

            if (Input.GetKey(_inputs.key_m_right))
                MoveRight(true);
        }
    }

    void LateUpdate()
    {
        if (UpdatePause)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnMovementStateChanged(_input.magnitude > 0);
        }

        //Normalise the given input. 
        _input.Normalize();

        //Apply the given movement to the players rigidbody multiplied by the players speed. 
        _body.linearVelocity = _input * speed;

        // Rotate the player to face the nearest cardinal direction
        if (_input != Vector3.zero)
        {
            Vector3 cardinalDirection = GetNearestCardinalDirection(_input);
            transform.rotation = Quaternion.LookRotation(cardinalDirection, Vector3.up);
        }

        //Cache the last input (possible to be used for attacks).
        //TODO: Remove later if unused. 
        _lastInput = _input;

        //Reset the current input. 
        _input = Vector3.zero;
    }

    private Vector3 GetNearestCardinalDirection(Vector3 input)
    {
        Vector3[] cardinalDirections = new Vector3[]
        {
            Vector3.forward,  // Up
            Vector3.back,     // Down
            Vector3.left,     // Left
            Vector3.right     // Right
        };

        Vector3 nearestDirection = Vector3.zero;
        float maxDot = -Mathf.Infinity;

        foreach (Vector3 direction in cardinalDirections)
        {
            float dot = Vector3.Dot(input.normalized, direction);
            if (dot > maxDot)
            {
                maxDot = dot;
                nearestDirection = direction;
            }
        }

        return nearestDirection;
    }
}