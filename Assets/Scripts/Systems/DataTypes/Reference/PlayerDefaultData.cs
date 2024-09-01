using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purrcifer.Data.Player.Defaults
{
    public class PlayerDefaultData
    {
        public const int MIN_HEALTH = 0; 
        public const int MAX_HEALTH = 3; 
        public const int CURRENT_HEALTH = 3; 
        public const float BASE_DAMAGE = 1; 
        public const float BASE_MULTIPLIER = 1; 
        public const float CRITICAL_HIT_DAMAGE = 3; 
        public const float CRITICAL_HIT_CHANCE = 10; 
    }

    public class DefaultGameStateData
    {
        public const int CURRENT_LEVEL = 0; 
    }

    public class DefaultSettingsData
    {
        public const int MASTER_VOLUME = 50;
        public const int SFX_VOLUME = 50;
        public const int UI_VOLUME = 50;
        public const int BGM_VOLUME = 50;
    }
}
