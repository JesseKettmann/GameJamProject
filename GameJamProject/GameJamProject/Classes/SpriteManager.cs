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
        public static Dictionary<string, SpriteFont> fonts;
        private static int depthMin = 0;
        private static int depthMax = 1;

        public static void Initialise()
        {
            textures = new Dictionary<string, Texture2D>();
            sprites = new Dictionary<string, Sprite>();
            fonts = new Dictionary<string, SpriteFont>();
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

        public static void AddFont(string name, string _font)
        {
            SpriteFont font = Game1.gameInstance.Content.Load<SpriteFont>("Fonts/" + _font);
            fonts.Add(name, font);
        }

        public static SpriteFont GetFont(string name)
        {
            SpriteFont f;
            if(fonts.TryGetValue(name, out f))
            {
                return f;
            } else
            {
                return null;
            }
        }


        // Default drawing method
        public static void DrawSprite(SpriteBatch spriteBatch, string _sprite, Vector2 position, Color color, int depth = 0, float rotation = 0, SpriteEffects spriteEffect = SpriteEffects.None, float scale = 0)
        {
            depthMin = Math.Min(depthMin, depth);
            depthMax = Math.Max(depthMax, depth);
            float _depth = (depth - (float)depthMin) / (depthMax - depthMin);
            Sprite sprite = sprites[_sprite];
            if (scale == 0)
                scale = Game1.pixelScale;
            spriteBatch.Draw(sprite.Texture, position, sprite.Source, color, -rotation, sprite.Origin, scale, spriteEffect, _depth);
        }
    }
}