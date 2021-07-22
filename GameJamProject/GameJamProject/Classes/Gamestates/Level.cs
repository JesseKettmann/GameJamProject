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
                if (dragon.length - 3 < value / 25)
                {
                    dragon.length++;
                }
            }
        }
        public float difficulty = 0.5f;
        private int _score = 0;
        public Dragon dragon;
        float boundary = 0;
        float cursor = 0;
        bool dead = false;
        bool newHighscore = false;
        bool paused = false;

        ScoreText scoreText;
        ScoreText subScoreText;
        Menu menu;

        Text deathText;
        Text highScore;
        Text PausedText;
        Text helpText;

        float helpAlpha = -2f;


        public Level()
        {
            dragon = new Dragon(new Vector2(-8 * Game1.pixelScale, Game1.gameInstance.viewSize.Y / 2));
            objects.Add(dragon);
            Camera.Initialise(Game1.gameInstance.viewSize, Game1.gameInstance.viewSize.ToVector2() / 2);
            Generator.Initialise();

            scoreText = new ScoreText(new Vector2(Game1.gameInstance.viewSize.X/2, 124), SpriteManager.GetFont("ScoreFont"));
            scoreText.SetText(score.ToString());
            scoreText.SetColor(Game1.gameInstance.White);

            subScoreText = new ScoreText(new Vector2(Game1.gameInstance.viewSize.X / 2, 60), SpriteManager.GetFont("MediumFont"));
            subScoreText.SetText("Score:");
            subScoreText.SetColor(Game1.gameInstance.White);

            menu = new Menu(Game1.gameInstance.viewSize.ToVector2()/2f, SpriteManager.GetFont("MediumFont"));
            deathText = new Text(Game1.gameInstance.viewSize.ToVector2() / 2f, SpriteManager.GetFont("MediumFont"));
            deathText.SetText("Press [Space] to continue");
            deathText.SetColor(Game1.gameInstance.White);

            highScore = new Text(new Vector2(Game1.gameInstance.viewSize.X/2f, 170), SpriteManager.GetFont("MediumFont"));
            highScore.SetText("Highscore: ");
            highScore.SetColor(Game1.gameInstance.White);

            PausedText = new Text(new Vector2(Game1.gameInstance.viewSize.X / 2f, 170), SpriteManager.GetFont("MediumFont"));
            PausedText.SetText("Game Paused");
            PausedText.SetColor(Game1.gameInstance.Black);

            helpText = new Text(new Vector2(Game1.gameInstance.viewSize.X / 2f, 170), SpriteManager.GetFont("MediumFont"));
            helpText.SetText("Hold [Space] to fly up");
            helpText.SetColor(Game1.gameInstance.Black * helpAlpha);
        }

        float deathTime = 0.0f;

        public override void Update(GameTime gameTime)
        {
            boundary = Math.Max(dragon.Position.X + (menu.playing ? 500 : 360), boundary);
            Camera.targetX = Math.Max(boundary * 0.2f + (dragon.Position.X + (menu.playing ? 500 : 360)) * 0.8f, Game1.gameInstance.viewSize.X / 2);
            Camera.UpdateShake(gameTime);
            if (dragon.alive && !paused)
                Camera.Update(gameTime);
            scoreText.SetText(score.ToString());
            scoreText.Update(gameTime);

            if (!paused)
                base.Update(gameTime);

            if(!dead && !dragon.alive)
            {
                dead = true;
                if(score > HighscoreManager.Highscore)
                {
                    newHighscore = true;
                    HighscoreManager.SetHighscore(score);
                }
            }

            // Spawn buildings
            if (menu.playing)
            {
                dragon.canStart = true;
                if (dragon.started)
                {
                    if (Camera.Location.X + Game1.gameInstance.viewSize.X > cursor)
                    {
                        cursor = Generator.SpawnOutpost(cursor, objects, difficulty);
                    }
                    if (!dead && Input.KeyPressed(Keys.Escape))
                    {
                        paused = !paused;
                        SoundManager.PlaySoundEffect(paused ? "pause" : "unpause");
                    }
                    helpText.SetColor(Game1.gameInstance.Black * 0);
                } else
                {
                    helpAlpha = Math.Min(helpAlpha + 0.67f * (float)gameTime.ElapsedGameTime.TotalSeconds, 1);
                    helpText.SetColor(Game1.gameInstance.Black * helpAlpha);
                }
            } else
            {
                menu.Update(gameTime);
            }
            if (!dragon.alive)
            {
                paused = false;
                deathTime = (float)gameTime.TotalGameTime.TotalSeconds - dragon.deadTime;

                if(deathTime > 1f)
                {
                    if (Input.KeyPressed(Keys.Space))
                    {
                        SoundManager.PlaySoundEffect("start", 0.7f);
                        Game1.gameInstance.gamestate = new Level();
                    }

                }

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
                subScoreText.Draw(spriteBatch);
                scoreText.Draw(spriteBatch);

            }

            if (!dragon.alive)
            {
                if (deathTime > 1)
                {
                    deathText.Draw(spriteBatch);
                    int highScore = HighscoreManager.Highscore;
                    if (highScore > -1)
                    {
                        if (!newHighscore)
                        {
                            this.highScore.SetText("Highscore: " + highScore);
                        }
                        else
                        {
                            this.highScore.SetText("New Highscore!!!");
                        }
                        this.highScore.Draw(spriteBatch);
                    }
                }
            }
            else
            {
                if (paused)
                    PausedText.Draw(spriteBatch);
                else helpText.Draw(spriteBatch);
            }

            base.DrawUI(spriteBatch);
        }
    }
}
