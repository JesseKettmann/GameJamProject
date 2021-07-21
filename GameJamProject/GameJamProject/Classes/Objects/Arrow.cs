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
        public float speed = 0.6f;
        private float direction;
        public int depth = -9690;
        public bool frozen;

        public Arrow(Vector2 position, float direction) : base(position)
        {
            this.direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            if (!frozen)
                Position -= new Vector2((float)Math.Cos(direction) * speed, -(float)Math.Sin(direction) * speed) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Position.X < Camera.Location.X - 2000)
                Despawn();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteManager.DrawSprite(spriteBatch, "SprArrow", Position, Color.White, depth, direction, SpriteEffects.None);
        }
    }
}
