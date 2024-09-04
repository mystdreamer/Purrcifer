using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class DefaultInputs
{
    public const KeyCode KEY_M_UP = KeyCode.W;
    public const KeyCode KEY_M_DOWN = KeyCode.S;
    public const KeyCode KEY_M_RIGHT = KeyCode.D;
    public const KeyCode KEY_M_LEFT = KeyCode.A;

    public const KeyCode KEY_A_UP = KeyCode.UpArrow;
    public const KeyCode KEY_A_DOWN = KeyCode.DownArrow;
    public const KeyCode KEY_A_RIGHT = KeyCode.RightArrow;
    public const KeyCode KEY_A_LEFT = KeyCode.LeftArrow;
    public const KeyCode KEY_MENU_A = KeyCode.Space;

    public const KeyCode CTLR_A = KeyCode.Joystick1Button0;
    public const KeyCode CTLR_B = KeyCode.Joystick1Button1;
    public const KeyCode CTLR_X = KeyCode.Joystick1Button2;
    public const KeyCode CTLR_Y = KeyCode.Joystick1Button3;

    public const PInputIdentifier AXIS_M_LEFT = PInputIdentifier.AXIS_LEFT_STICK;
    public const PInputIdentifier AXIS_A_RIGHT = PInputIdentifier.AXIS_RIGHT_STICK;
    public const PInputIdentifier AXIS_DPAD = PInputIdentifier.AXIS_DPAD;
}
