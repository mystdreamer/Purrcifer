using UnityEngine;

namespace Purrcifer.Data.Defaults
{
    #region World Data. 

    /// <summary>
    /// Enum states for the world. 
    /// </summary>
    [System.Serializable]
    public enum WorldStateEnum : int
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
        public const float WORLD_TIMESCALE_MINUTE = 60;

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

    public class DefaultRoomData
    {
        public const int DEFAULT_WIDTH = 36;
        public const int DEFAULT_HEIGHT = 20;
    }
    #endregion

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

    /// <summary>
    /// Default data for the player. 
    /// </summary>
    public class PlayerDefaultData
    {
        public const int MIN_HEALTH = 0;
        public const int MAX_HEALTH = 3;
        public const int CURRENT_HEALTH = 3;
        public const float BASE_DAMAGE = 1;
        public const float BASE_MULTIPLIER = 1;
        public const float CRITICAL_HIT_DAMAGE = 3;
        public const float CRITICAL_HIT_CHANCE = 10;
        public const bool SITEM_SWORD_UNLOCKED = false;
        public const bool SITEM_TIMETWISTER_UNLOCKED = false;
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

