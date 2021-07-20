using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Object
    {
        public Vector2 Position;
        public bool despawned = false;
        
        public Object(Vector2 position)
        {
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public void Despawn()
        {
            despawned = true;
        }
    }
}
