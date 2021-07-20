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
        Dragon dragon;
        float boundary = 0;
        float cursor = 0;

        public Level()
        {
            score = 0;
            dragon = new Dragon(new Vector2(-8 * Game1.pixelScale, Game1.gameInstance.viewSize.Y / 2));
            objects.Add(dragon);
            Camera.Initialise(Game1.gameInstance.viewSize, Game1.gameInstance.viewSize.ToVector2() / 2);
            Generator.Initialise();
        }

        public override void Update(GameTime gameTime)
        {
            boundary = Math.Max(dragon.Position.X + 300, boundary);
            Camera.targetX = Math.Max(boundary * 0.2f + (dragon.Position.X + 300) * 0.8f, Game1.gameInstance.viewSize.X / 2);
            Camera.Update(gameTime);
            base.Update(gameTime);

            // Spawn buildings
            if (dragon.started)
            {
                if (Camera.Location.X + Game1.gameInstance.viewSize.X > cursor)
                {
                    cursor = Generator.SpawnOutpost(cursor, objects);
                }
            }
            cursor = Math.Max(Camera.Location.X + Game1.gameInstance.viewSize.X / 2 + 100, cursor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float skyPos = Camera.Location.X - (Camera.Location.X % (300 * Game1.pixelScale));
            for (int i = -1; i < 2; i ++)
            {
                SpriteManager.DrawSprite(spriteBatch, "SprSky", new Vector2(skyPos + 300 * Game1.pixelScale * i, 0), Color.White, -10000);
                SpriteManager.DrawSprite(spriteBatch, "SprGround", new Vector2(skyPos + 300 * Game1.pixelScale * i, 169 * Game1.pixelScale), Color.White, -9000);
            }
            base.Draw(spriteBatch);
        }
    }
}
