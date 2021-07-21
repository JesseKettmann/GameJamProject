using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Arrow : Object
    {
        public static float gravity = 1;
        public static float speed = 1;

        public Arrow(Vector2 position, Vector2 direction) : base(position)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X < Camera.Location.X - 2000)
                Despawn();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
