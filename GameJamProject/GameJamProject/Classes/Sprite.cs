using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Origin { get; private set; }
        public Rectangle Source { get; private set; }

        public Sprite(Texture2D texture, Vector2 origin, Rectangle source)
        {
            Texture = texture;
            Origin = origin;
            Source = source;
        }
    }
}
