using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        List<Text> menuItems;
        List<string> menuLabels;

        Text pointer;


        public Menu(Vector2 position, SpriteFont font) : base(position)
        {
            this.font = font;

            menuLabels = new List<string>();
            menuLabels.Add("Play");
            menuLabels.Add("Exit");

            menuItems = new List<Text>();

            Text playText;
            Text exitText;
            playText = new Text(position + new Vector2(0, 180), font);
            playText.SetText(menuLabels[0]);
            playText.SetColor(new Color(25, 14, 14));
            exitText = new Text(position + new Vector2(0, 230), font);
            exitText.SetText(menuLabels[1]);
            exitText.SetColor(new Color(25, 14, 14));

            pointer = new Text(position + new Vector2(-50, 180), font);
            pointer.SetText(">");
            pointer.SetColor(new Color(25, 14, 14));

            menuItems.Add(playText);
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


                }
                else if (Input.KeyPressed(Keys.Up) || Input.KeyPressed(Keys.W))
                {
                    selectedIndex--;
                    UpdateSelected();
                }

                if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
                {
                    if (selectedIndex == 1)
                    {
                        Game1.gameInstance.Exit();
                    }
                    else if (selectedIndex == 0)
                    {
                        playing = true;
                    }
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

            pointer.Position = Position + new Vector2(-50, 180 + (selectedIndex * 50));
        }
    }
}