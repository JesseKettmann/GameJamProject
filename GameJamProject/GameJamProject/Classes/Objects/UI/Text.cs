using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    class Text : Object
    {

        private string text = "";
        private SpriteFont font;
        private Color color = Color.White;

        public Text(Vector2 position, SpriteFont font) : base(position)
        {
            this.font = font;
        }

        public virtual void SetText(string text)
        {
            this.text = text;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 size = font.MeasureString(text);
            spriteBatch.DrawString(font, text, (Position-size/2f).ToPoint().ToVector2(), color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



    }
}
