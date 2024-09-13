using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Purrcifer.Window.Management
{
    public static class GameWindowManagement
    {
        public static Vector2Int[] aspectRatios = new Vector2Int[]
        {
            new Vector2Int(1366, 768),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440),
            new Vector2Int(3840, 2160),
            new Vector2Int(7680, 4320),
        };

        public static void ManageWindowFullscreen()
        {
            // Toggle fullscreen mode
            Screen.fullScreen = !Screen.fullScreen;
        }

        public static void ApplyAspectRatio(int x, int y)
        {
            Vector2Int param = new Vector2Int(x, y);

            foreach (Vector2Int ratio in aspectRatios)
            {
                if (ratio == param)
                    Screen.SetResolution(x, y, Screen.fullScreen);
            }
        }
    }
}
