using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameJamProject
{
    static class HighscoreManager
    {
        public static int Highscore;

        public static void SetHighscore(int highscore)
        {
            try
            {
                File.WriteAllText("Highscore.txt", highscore.ToString());
                Highscore = highscore;
            }
            catch { return;  }
        }

        public static void RetrieveHighscore()
        {
            try
            {
                string score = File.ReadAllText("Highscore.txt");
                Highscore = int.Parse(score);
            }
            catch
            {
                Highscore = -1;
            }
        }
    }
}
