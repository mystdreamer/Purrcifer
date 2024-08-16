using System;
using UnityEngine;

public class MovementSysTest : MonoBehaviour
{

    public Vector3 direction = Vector2.zero;
    public Vector3 input = Vector2.zero;
    public Rigidbody body;
    public float speed = 100; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSysTest.Instance.GetAction(ActionType.M_LEFT).DoAction += MoveRight;
        InputSysTest.Instance.GetAction(ActionType.M_RIGHT).DoAction += MoveLeft;
        InputSysTest.Instance.GetAction(ActionType.M_UP).DoAction += MoveDown;
        InputSysTest.Instance.GetAction(ActionType.M_DOWN).DoAction += MoveUp;
    }

    private void OnDisable()
    {
        InputSysTest.Instance.GetAction(ActionType.M_LEFT).DoAction -= MoveRight;
        InputSysTest.Instance.GetAction(ActionType.M_RIGHT).DoAction -= MoveLeft;
        InputSysTest.Instance.GetAction(ActionType.M_UP).DoAction -= MoveDown;
        InputSysTest.Instance.GetAction(ActionType.M_DOWN).DoAction -= MoveUp;
    }

    private void MoveRight(bool result)
    {
        if (result)
            input += new Vector3(-1, 0, 0);
    }

    private void MoveLeft(bool result)
    {
        if (result)
            input += new Vector3(1, 0, 0);
    }

    private void MoveUp(bool result)
    {
        Debug.Log("Input Right: " + result);
        if (result)
            input += new Vector3(0, 0, -1);
    }

    private void MoveDown(bool result)
    {
        if (result)
            input += new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        input.Normalize();
        body.linearVelocity = input * speed;
        input = Vector3.zero;
    }
}
