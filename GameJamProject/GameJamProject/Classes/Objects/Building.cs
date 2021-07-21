using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Building : Object
    {
        protected string Sprite;
        protected bool flipped;
        
        public Building(Vector2 position, string sprite, bool flipped = false) : base(position)
        {
            Sprite = sprite;
            this.flipped = flipped;
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X < Camera.Location.X - 2000)
                Despawn();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteManager.DrawSprite(spriteBatch, Sprite, Position, Color.White, -9900, 0, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
