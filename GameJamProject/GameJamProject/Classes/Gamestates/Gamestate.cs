﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Gamestate
    {
        protected List<Object> objects;

        public Gamestate()
        {
            objects = new List<Object>();
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
                objects[i].Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
                objects[i].Draw(spriteBatch);
        }
    }
}
