using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    static class Settings
    {

        public static float volume = 1f;

        public static void ScrollVolume()
        {
            volume += 0.1f;
            volume %= 1.1f;
        }



    }
}
