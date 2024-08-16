using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    M_LEFT, 
    M_RIGHT, 
    M_UP, 
    M_DOWN,
    A_UP, 
    A_DOWN,
    A_LEFT, 
    A_RIGHT, 
    A_INTERACT
}

public interface IPlayerAction
{
    void DoAction(KeyCode key, bool state);
}

[System.Serializable]
public class PlayerInputData
{
    public string actionName;
    public KeyCode key;
    public KeyCode Button;
    public delegate void MovementAction(bool result);
    public event MovementAction DoAction;
    public ActionType type;

    public void Command_Key()
    {
        if(Input.GetKey(key))
            DoAction?.Invoke(true);
        if(Input.GetKeyUp(key))
            DoAction?.Invoke(false);
    }
}

public class InputSysTest : MonoBehaviour
{
    private static InputSysTest instance;
    public List<PlayerInputData> keycodeData;

    public static InputSysTest Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    public PlayerInputData GetAction(ActionType type)
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            if (keycodeData[i].type == type)
                return keycodeData[i];
        }

        return null;
    }

    public void UpdateInput()
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            keycodeData[i].Command_Key();
        }
    }
}
