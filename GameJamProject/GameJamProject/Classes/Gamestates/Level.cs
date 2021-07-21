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
        public int score {
            get {
                return _score;
            }
            set {
                _score = value;
                scoreText.Bounce();
                if (dragon.length - 6 < (Math.Log(value / 6 + 50) / Math.Log(1.04f)) - 100)
                {
                    dragon.length++;
                }
            }
        }
        private int _score = 0;
        public static Dragon dragon;
        float boundary = 0;
        float cursor = 0;

        ScoreText scoreText;
        Menu menu;
        public Level()
        {
            dragon = new Dragon(new Vector2(-8 * Game1.pixelScale, Game1.gameInstance.viewSize.Y / 2));
            objects.Add(dragon);
            Camera.Initialise(Game1.gameInstance.viewSize, Game1.gameInstance.viewSize.ToVector2() / 2);
            Generator.Initialise();

            scoreText = new ScoreText(new Vector2(Game1.gameInstance.viewSize.X/2, 100), SpriteManager.GetFont("BigFont"));
            scoreText.SetText(score.ToString());
            scoreText.SetColor(Game1.gameInstance.White);

            menu = new Menu(Game1.gameInstance.viewSize.ToVector2()/2f, SpriteManager.GetFont("MediumFont"));
        }

        public override void Update(GameTime gameTime)
        {

            boundary = Math.Max(dragon.Position.X + 360, boundary);
            Camera.targetX = Math.Max(boundary * 0.2f + (dragon.Position.X + 360) * 0.8f, Game1.gameInstance.viewSize.X / 2);
            if (dragon.alive)
                Camera.Update(gameTime);
            scoreText.SetText(score.ToString());
            scoreText.Update(gameTime);

            base.Update(gameTime);

            // Spawn buildings
            if (menu.playing)
            {
                dragon.canStart = true;
                if (dragon.started)
                {
                    if (Camera.Location.X + Game1.gameInstance.viewSize.X > cursor)
                    {
                        cursor = Generator.SpawnOutpost(cursor, objects);
                    }
                }

            } else
            {
                menu.Update(gameTime);
            }
            cursor = Math.Max(Camera.Location.X + Game1.gameInstance.viewSize.X / 2 + 100, cursor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            float skyPos = Camera.Location.X - ((Camera.Location.X / 2) % (300 * Game1.pixelScale));
            float groundPos = Camera.Location.X - (Camera.Location.X % (300 * Game1.pixelScale));
            for (int i = -1; i < 2; i++)
            {
                SpriteManager.DrawSprite(spriteBatch, "SprSky", new Vector2(skyPos + 300 * Game1.pixelScale * i, 0), Color.White, -10000);
                SpriteManager.DrawSprite(spriteBatch, "SprGround", new Vector2(groundPos + 300 * Game1.pixelScale * i, 169 * Game1.pixelScale), Color.White, -9000);
            }
            base.Draw(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            if (!menu.playing)
            {
                menu.Draw(spriteBatch);
            }
            else
            {
                scoreText.Draw(spriteBatch);

            }
            base.DrawUI(spriteBatch);
        }
    }
}
