using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Archer : Destructible
    {
        int state = 0;
        float timeToFire;
        float aim = (float)Math.PI / 4;
        float aimTarget = -(float)Math.PI / 4;
        string bowSprite = "SprBowKnock";

        public Archer(Vector2 position, int depthAdd = 0) : base(position, "SprArcherLowered", 15, -9750 + depthAdd)
        {
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!destroyed)
            {
                if (Camera.Location.X + Camera.Bounds.Width / 2 > Position.X - 150)
                {
                    switch (state)
                    {
                        case 0:
                            aim = (float)Math.PI / 4;
                            Sprite = "SprArcherLowered";
                            timeToFire -= deltaTime / 1000;
                            if (timeToFire <= 0)
                                state = 1;
                            break;
                        case 1:
                            if (Game1.gameInstance.gamestate as Level != null)
                            {
                                //Dragon dragon = (Game1.gameInstance.gamestate as Level).dr
                            }
                            //aimTarget = Extensions.GetAngle(Position + new Vector2(0, -21 * Game1.pixelScale), );
                            aim += (aimTarget - aim) * 0.001f * deltaTime;
                            Sprite = "SprArcherTorso";
                            bowSprite = "SprBowKnock";
                            break;
                    }
                } else
                {
                    Reset();
                }
                
            } else
            {
                Sprite = "SprArcherLowered";
                state = 0;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenshake = Vector2.Zero;
            if (hitstop >= 0.0001f)
                screenshake = new Vector2(Game1.random.Next(-6, 6), Game1.random.Next(-6, 6));
            SpriteManager.DrawSprite(spriteBatch, Sprite, Position + screenshake + (destroyed ? new Vector2(-14 * Game1.pixelScale) : Vector2.Zero), Color.White, depth, destroyed ? (float)(-Math.PI / 2) : 0, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            switch (state)
            {
                case 1:
                case 2:
                case 3:
                    SpriteManager.DrawSprite(spriteBatch, bowSprite, Position + new Vector2(0, -21 * Game1.pixelScale), Color.White, depth - 2, aim, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                    break;
            }
        }

        private void Reset()
        {
            state = 0;
            timeToFire = 0.6f + (float)Game1.random.NextDouble() * 1f;
        }
    }
}
