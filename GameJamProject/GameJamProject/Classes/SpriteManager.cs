using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public static class SpriteManager
    {
        private static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, Sprite> sprites;
        public static int depthMin = 0;
        public static int depthMax = 1;

        public static void Initialise()
        {
            textures = new Dictionary<string, Texture2D>();
            sprites = new Dictionary<string, Sprite>();
        }

        public static void AddTexture(string name, string fileName)
        {
            Texture2D texture = Game1.gameInstance.Content.Load<Texture2D>("Sprites/" + fileName);
            textures.Add(name, texture);
        }

        public static void AddSprite(string name, string _texture, Vector2 origin, Rectangle source)
        {
            Texture2D texture = textures[_texture];
            Sprite sprite = new Sprite(texture, origin, source);
            sprites.Add(name, sprite);
        }

        public static void AddSprite(string name, string _texture, Vector2 origin)
        {
            Texture2D texture = textures[_texture];
            Sprite sprite = new Sprite(texture, origin, new Rectangle(0, 0, texture.Width, texture.Height));
            sprites.Add(name, sprite);
        }

        // Default drawing method
        public static void DrawSprite(SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color, int depth = 0, float rotation = 0, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            depthMin = Math.Min(depthMin, depth);
            depthMax = Math.Max(depthMax, depth);
            float _depth = (depth - depthMin) / (depthMax - depthMin);
            spriteBatch.Draw(sprite.Texture, position, sprite.Source, color, -rotation, sprite.Origin, 1f, spriteEffect, _depth);
        }
    }
}