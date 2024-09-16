using UnityEngine;

namespace Purrcifer.Data.Defaults
{
    #region World Data. 

    /// <summary>
    /// Enum states for the world. 
    /// </summary>
    [System.Serializable]
    public enum WorldState : int
    {
        WORLD_START = 0,
        WORLD_WITCHING = 1,
        WORLD_HELL = 2
    }

    /// <summary>
    /// Class containing timings used to control the time of the world. 
    /// </summary>
    public class WorldTimings
    {
        /// <summary>
        /// The scale of time per minute. 
        /// </summary>
        public const float WORLD_TIMESCALE_MINUTE = 10;

        /// <summary>
        /// When world time actually starts. 
        /// </summary>
        public const float WORLD_START_TIME = 0.0f;

        /// <summary>
        /// The threshold for ticking over into witching hour. 
        /// </summary>
        public const float WORLD_WITCHING_HOUR_TIME = 5.0F;

        /// <summary>
        /// The threshold for ticking over into hell hour. 
        /// </summary>
        public const float WORLD_HELL_HOUR_TIME = 10.0F;
    }

    /// <summary>
    /// Default data for the camera.
    /// </summary>
    public class DefaultCameraData
    {
        public const float STEP_TRANSITION_TIME = 0.80F;
        public const float PLAYER_STEP_DISTANCE = 1.3F; 
    }

    /// <summary>
    /// Default data for rooms.
    /// </summary>
    public class DefaultRoomData
    {
        public const float DEFAULT_WIDTH = 37.3F;
        public const float DEFAULT_HEIGHT = 24F;
    }
    #endregion

    /// <summary>
    /// Default definitions for player inputs. 
    /// </summary>
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
        public const KeyCode KEY_UTIL_ACTION_A = KeyCode.Q;
        public const KeyCode KEY_UTIL_ACTION_B = KeyCode.E;

        public const KeyCode CTLR_A = KeyCode.Joystick1Button0;
        public const KeyCode CTLR_B = KeyCode.Joystick1Button1;
        public const KeyCode CTLR_X = KeyCode.Joystick1Button2;
        public const KeyCode CTLR_Y = KeyCode.Joystick1Button3;
        public const KeyCode CTLR_UTIL_ACTION_A = KeyCode.Joystick1Button4;
        public const KeyCode CTLR_UTIL_ACTION_B = KeyCode.Joystick1Button5;

        public const PInputIdentifier AXIS_M_LEFT = PInputIdentifier.AXIS_LEFT_STICK;
        public const PInputIdentifier AXIS_A_RIGHT = PInputIdentifier.AXIS_RIGHT_STICK;
        public const PInputIdentifier AXIS_D_PAD = PInputIdentifier.AXIS_DPAD;
    }

    /// <summary>
    /// Class containing default game state data. 
    /// </summary>
    public class DefaultGameStateData
    {
        public const int CURRENT_LEVEL = 0;
    }

    /// <summary>
    /// Class containing default settings data. 
    /// </summary>
    public class DefaultSettingsData
    {
        public const int MASTER_VOLUME = 50;
        public const int SFX_VOLUME = 50;
        public const int UI_VOLUME = 50;
        public const int BGM_VOLUME = 50;
    }
}

