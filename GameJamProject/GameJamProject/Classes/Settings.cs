using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameJamProject
{
    static class Settings
    {

        public static float volume = 1f;

        public static void ScrollVolume()
        {
            SetVolume(volume - 0.1f);
        }
        public static void SetVolume(float vol)
        {
            volume = Math.Abs(vol);
            if (vol < -0.01f)
                volume = 1f;
            MediaPlayer.Volume = volume * 0.2f;
            try
            {
                File.WriteAllText("Volume.txt", Math.Round(volume*100).ToString());
            }
            catch { return; }
        }

        public static void RetrieveVolume()
        {
            try
            {
                string score = File.ReadAllText("Volume.txt");
                volume = int.Parse(score)/100f;
                MediaPlayer.Volume = volume*0.2f;
            }
            catch
            {
                volume = 0.7f;
                MediaPlayer.Volume = volume * 0.2f;

            }
        }
    }
}
