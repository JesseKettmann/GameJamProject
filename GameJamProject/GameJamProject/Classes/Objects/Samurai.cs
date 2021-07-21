using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Samurai : Destructible
    {
        bool phase = false;
        float phaseExtra;
        float walkSpeed = 0.1f;

        public Samurai(Vector2 position, int depthAdd = 0) : base(position, "SprSamurai1", 12, -9700 + depthAdd)
        {
            phaseExtra = (float)Game1.random.NextDouble();
            sound = "kill";
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!destroyed)
            {
                if (Camera.Location.X + Camera.Bounds.Width / 2 > Position.X - 150)
                Position.X -= deltaTime * walkSpeed;
                SetHitbox();
                phase = ((gameTime.TotalGameTime.TotalSeconds + phaseExtra) * 4) % 2 < 1f;
            } else
            {
                phase = false;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenshake = Vector2.Zero;
            if (hitstop >= 0.0001f)
                screenshake = new Vector2(Game1.random.Next(-6, 6), Game1.random.Next(-6, 6));
            SpriteManager.DrawSprite(spriteBatch, "SprSamurai" + (phase ? 1 : 2), Position + screenshake + (destroyed ? new Vector2(-14 * Game1.pixelScale) : Vector2.Zero), Color.White, depth, destroyed ? (float)(-Math.PI / 2) : 0, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
