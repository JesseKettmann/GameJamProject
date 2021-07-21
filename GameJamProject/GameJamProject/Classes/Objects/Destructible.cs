using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Destructible : Building
    {
        protected int depth;
        int score;
        protected bool destroyed = false;
        protected float hitstop = 0;
        Rectangle hitbox;
        float gravity = 0;
        
        public Destructible(Vector2 position, string sprite, int score, int depth = 0, bool flipped = false) : base(position, sprite, flipped)
        {
            this.depth = depth;
            this.score = score;
            SetHitbox();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (destroyed)
            {
                if (hitstop < 0.0001f)
                {
                    Position.Y += gravity;
                    gravity += 1;
                    if (Position.Y > 2000)
                        Despawn();
                }
                else hitstop -= deltaTime;
            } else
            {
                Level level = Game1.gameInstance.gamestate as Level;
                if (level == null)
                    return;
                Rectangle dragonHitbox = new Rectangle((int)(level.dragon.Position.X - 8 * Game1.pixelScale), (int)(level.dragon.Position.Y - 8 * Game1.pixelScale), (int)(16 * Game1.pixelScale), (int)(16 * Game1.pixelScale));
                if (hitbox.Intersects(dragonHitbox))
                {
                    SoundManager.PlaySoundEffect("hit", 0.4f);
                    Camera.Shake(5, 0.1f, 1);
                    (Game1.gameInstance.gamestate as Level).dragon.hitstop = 50f;
                    if (Game1.gameInstance.gamestate as Level != null)
                        (Game1.gameInstance.gamestate as Level).score += score;
                    hitstop = 115f;
                    destroyed = true;
                    gravity = -10;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenshake = Vector2.Zero;
            if (hitstop >= 0.0001f)
                screenshake = new Vector2(Game1.random.Next(-6, 6), Game1.random.Next(-6, 6));
            SpriteManager.DrawSprite(spriteBatch, Sprite, Position + screenshake, Color.White, depth, 0, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        protected void SetHitbox()
        {
            Sprite spriteFull = SpriteManager.sprites[Sprite];
            hitbox = new Rectangle((int)(Position.X - spriteFull.Origin.X * Game1.pixelScale), (int)(Position.Y - spriteFull.Origin.Y * Game1.pixelScale), (int)(spriteFull.Source.Width * Game1.pixelScale), (int)(spriteFull.Source.Height * Game1.pixelScale));
        }
    }
}
