using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    class Menu : Object
    {

        private SpriteFont font;
        public bool playing = false;
        int selectedIndex = 0;

        Text CreditsText1;
        Text CreditsText2;

        List<Text> menuItems;
        List<string> menuLabels;

        Text pointer;


        public Menu(Vector2 position, SpriteFont font) : base(position)
        {
            this.font = font;

            menuLabels = new List<string>();
            menuLabels.Add("Play");
            menuLabels.Add("Music");
            menuLabels.Add("Exit");

            CreditsText1 = new Text(position + new Vector2(0, 360), SpriteManager.GetFont("SmallFont"));
            CreditsText1.SetText("Created by Jesse Kettmann and Luuk van den Hoven for the eight Bored Pixels Jam, 2021");
            CreditsText1.SetColor(Game1.gameInstance.Orange);
            CreditsText2 = new Text(position + new Vector2(0, 355), font);
            CreditsText2.SetText("");
            CreditsText2.SetColor(Game1.gameInstance.Orange);

            menuItems = new List<Text>();

            Text playText;
            Text musicText;
            Text exitText;

            playText = new Text(position + new Vector2(0, 150), font);
            playText.SetText(menuLabels[0]);
            playText.SetColor(Game1.gameInstance.Black);
            musicText = new Text(position + new Vector2(0, 200), font);
            musicText.SetText(menuLabels[1]);
            musicText.SetColor(Game1.gameInstance.Black);
            exitText = new Text(position + new Vector2(0, 250), font);
            exitText.SetText(menuLabels[2]);
            exitText.SetColor(Game1.gameInstance.Black);

            pointer = new Text(position + new Vector2(-50, 150), font);
            pointer.SetText(">");
            pointer.SetColor(Game1.gameInstance.Black);

            menuItems.Add(playText);
            menuItems.Add(musicText);
            menuItems.Add(exitText);



            UpdateSelected();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!playing)
            {
                foreach(Text t in menuItems)
                {
                    t.Draw(spriteBatch);
                }
                pointer.Draw(spriteBatch);
                CreditsText1.Draw(spriteBatch);
                CreditsText2.Draw(spriteBatch);
            }



            //Vector2 size = font.MeasureString(text);
            //spriteBatch.DrawString(font, text, Position - size / 2f, color);
            //base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!playing)
            {
                if (Input.KeyPressed(Keys.Down) || Input.KeyPressed(Keys.S))
                {
                    selectedIndex++;
                    UpdateSelected();
                    SoundManager.PlaySoundEffect("select", 0.7f);
                }
                else if (Input.KeyPressed(Keys.Up) || Input.KeyPressed(Keys.W))
                {
                    selectedIndex--;
                    UpdateSelected();
                    SoundManager.PlaySoundEffect("select", 0.7f);
                }

                if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
                {
                    if (selectedIndex == 2)
                    {
                        Game1.gameInstance.Exit();
                    }
                    else if (selectedIndex == 0)
                    {
                        SoundManager.PlaySoundEffect("start", 0.7f);
                        playing = true;
                    } else if(selectedIndex == 1)
                    {
                        Settings.ScrollVolume();
                        SoundManager.PlaySoundEffect("volume", Settings.volume, 0, 0);
                        MediaPlayer.Volume = Settings.volume*0.2f;
                    }
                }
                if (selectedIndex == 1)
                {
                    menuItems[1].SetText("Music " + Math.Round((Settings.volume*100)) + "%");
                    pointer.Position = Position + new Vector2(-100, 150 + (selectedIndex * 50));
                }
                else
                {
                    menuItems[1].SetText("Music");

                }
            }
            base.Update(gameTime);
        }
        private void UpdateSelected()
        {
            if(selectedIndex < 0)
            {
                selectedIndex = menuItems.Count - 1;
            } else if(selectedIndex > menuItems.Count - 1)
            {
                selectedIndex = 0;
            }

            pointer.Position = Position + new Vector2(-50, 150 + (selectedIndex * 50));
        }
    }
}