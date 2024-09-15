using Purrcifer.Data.Defaults;
using UnityEngine;

namespace Purrcifer.Data.Player
{
    public class PlayerInputs
    {
        public KeyCode key_m_up;
        public KeyCode key_m_down;
        public KeyCode key_m_right;
        public KeyCode key_m_left;

        public KeyCode key_a_up;
        public KeyCode key_a_down;
        public KeyCode key_a_right;
        public KeyCode key_a_left;
        public KeyCode key_menu_a;
        public KeyCode key_util_action_a;
        public KeyCode key_util_action_b;

        public KeyCode ctlr_a;
        public KeyCode ctlr_b;
        public KeyCode ctlr_x;
        public KeyCode ctlr_y;
        public KeyCode ctlr_util_action_a;
        public KeyCode ctlr_util_action_b;

        public PInputIdentifier axis_m_left;
        public PInputIdentifier axis_a_right;
        public PInputIdentifier axis_d_pad;

        public PlayerInputs GetDefault()
        {
            return new PlayerInputs()
            {
                key_m_up = DefaultInputs.KEY_M_UP,
                key_m_down = DefaultInputs.KEY_M_DOWN,
                key_m_right = DefaultInputs.KEY_M_RIGHT,
                key_m_left = DefaultInputs.KEY_M_LEFT,
                key_a_up = DefaultInputs.KEY_A_UP,
                key_a_down = DefaultInputs.KEY_A_DOWN,
                key_a_right = DefaultInputs.KEY_A_RIGHT,
                key_a_left = DefaultInputs.KEY_A_LEFT,
                key_menu_a = DefaultInputs.KEY_MENU_A,
                key_util_action_a = DefaultInputs.KEY_UTIL_ACTION_A,
                key_util_action_b = DefaultInputs.KEY_UTIL_ACTION_B,
                ctlr_a = DefaultInputs.CTLR_A,
                ctlr_b = DefaultInputs.CTLR_B,
                ctlr_x = DefaultInputs.CTLR_X,
                ctlr_y = DefaultInputs.CTLR_Y,
                ctlr_util_action_a = DefaultInputs.CTLR_UTIL_ACTION_A,
                ctlr_util_action_b = DefaultInputs.CTLR_UTIL_ACTION_B,
                axis_m_left = DefaultInputs.AXIS_M_LEFT,
                axis_a_right = DefaultInputs.AXIS_A_RIGHT,
                axis_d_pad = DefaultInputs.AXIS_D_PAD,
            };
        }
    }
}
