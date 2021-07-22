using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Popup : Object
    {
        Text txt;
        float velocity = -0.7f;
        float spawnTime;
        float alive = 0;

        public Popup(Vector2 position, int nr, float spawnTime) : base(position)
        {
            txt = new Text(position, SpriteManager.GetFont("MediumFont"));
            txt.SetText(nr.ToString());
            txt.SetColor(Game1.gameInstance.White);

            this.spawnTime = spawnTime;
        }

        public override void Update(GameTime gameTime)
        {
            Level level = Game1.gameInstance.gamestate as Level;
            if (level == null)
                return;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position.Y += velocity * deltaTime;
            velocity *= 0.9f;
            alive = (float)gameTime.TotalGameTime.TotalSeconds - spawnTime;
            if (alive > 1f || level.dragon.alive == false)
                Despawn();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            txt.SetColor((alive % 0.2f > 0.1f) ? Game1.gameInstance.Black : Game1.gameInstance.White);
            txt.Position = Position;
            txt.Draw(spriteBatch);
        }
    }
}
