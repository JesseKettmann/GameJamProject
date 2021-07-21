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
        float coolDown = 0;
        float aim = (float)Math.PI / 4;
        float aimTarget = -(float)Math.PI / 4;
        float aimExtra;
        string bowSprite = "SprBowKnock";

        public Archer(Vector2 position, int depthAdd = 0) : base(position, "SprArcherLowered", 15, -9750 + depthAdd)
        {
            Reset();
            aimExtra = (float)Game1.random.NextDouble() - 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!destroyed)
            {
                Level level = Game1.gameInstance.gamestate as Level;
                Dragon dragon = null;
                if (level == null)
                    return;
                dragon = level.dragon;
                if (dragon == null)
                    return;
                if (Camera.Location.X + Camera.Bounds.Width / 2 > Position.X && dragon.Position.X < Position.X)
                {
                    timeToFire -= deltaTime / 1000;
                    coolDown -= deltaTime / 1000;
                    switch (state)
                    {
                        case 0:
                            aim = (float)Math.PI / 4;
                            Sprite = "SprArcherLowered";
                            if (timeToFire <= 0 && coolDown <= 0)
                            {
                                state = 1;
                                timeToFire = 0.2f;
                            }
                            break;
                        case 1:
                        case 2:
                            aimTarget = Extensions.GetAngle(Position + new Vector2(0, -21 * Game1.pixelScale), dragon.Position + new Vector2(0, -200)) + aimExtra;
                            aimTarget = MathHelper.Clamp(aimTarget, -(float)Math.PI * 0.65f, (float)Math.PI * 0.65f);
                            aim += (aimTarget - aim) * 0.0035f * deltaTime;
                            Sprite = "SprArcherTorso";
                            bowSprite = state == 1 ? "SprBowKnock" : "SprBowDrawn";
                            if (timeToFire <= 0 && dragon.alive)
                            {
                                state ++;
                                timeToFire = 0.2f;
                            }
                            if (dragon.Position.X > Position.X - 200)
                            {
                                state = 1;
                            }
                            break;
                        case 3:
                            Sprite = "SprArcherTorso";
                            bowSprite = "SprBowLoose";
                            if (timeToFire <= 0)
                            {
                                Reset();
                                level.objects.Add(new Arrow(Position + new Vector2(0, -21 * Game1.pixelScale), aim));
                                coolDown = 0.8f + (float)Game1.random.NextDouble();
                            }
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
            Sprite = "SprArcherLowered";
            state = 0;
            timeToFire = 0.1f + (float)Game1.random.NextDouble() * 0.5f;
        }
    }
}
