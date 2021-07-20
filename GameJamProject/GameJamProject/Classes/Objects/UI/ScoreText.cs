using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    class ScoreText : Text
    {
        const float BOUNCE_TIME = 0.08f;
        bool returnLerp = false;
        Vector2 originalPosition;
        float bounceTime;

        public ScoreText(Vector2 position, SpriteFont font) : base(position, font)
        {
            originalPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            //bounceTime =(float) Math.Max(0, bounceTime - gameTime.ElapsedGameTime.TotalSeconds);
            //if(bounceTime > 0)
            //{
            //    Vector2 target = returnLerp ? originalPosition : originalPosition + new Vector2(0, -0);
            //    Position = Vector2.Lerp(Position, target, (BOUNCE_TIME-bounceTime)/BOUNCE_TIME);
            //} else if(bounceTime == 0 && !returnLerp)
            //{
            //    bounceTime = BOUNCE_TIME;
            //    returnLerp = true;
            //}
            bounceTime = (float)Math.Max(0, bounceTime - gameTime.ElapsedGameTime.TotalSeconds);
            if (bounceTime > 0)
            {
                Position = Vector2.Lerp(Position, originalPosition, (BOUNCE_TIME-bounceTime)/BOUNCE_TIME);
            }


            base.Update(gameTime);
        }

        public override void SetText(string text)
        {

            base.SetText(text);
        }

        public void Bounce()
        {
            bounceTime = BOUNCE_TIME;
            Position = originalPosition + new Vector2(0, -30);
            returnLerp = false;
        }
    }
}
