using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Level : Gamestate
    {
        public int score;
        
        public Level()
        {
            score = 0;
        }
    }
}
