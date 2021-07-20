using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Camera
    {
        private float cameraSpeed = 1f;
        public Vector2 Location;

        public Rectangle Bounds { get; set; }
        public float targetX;

        public Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0f)) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        public Camera(Point viewport, Vector2 position)
        {
            Bounds = new Rectangle(0, 0, viewport.X, viewport.Y);
            Location = position;
            targetX = position.X;
        }

        public void Update(GameTime gameTime)
        {
            Location.X = MathHelper.Lerp(Location.X, targetX, cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            this.Location = Vector2.Round(this.Location);
            Game1.cameraMatrix = TransformMatrix;
        }
    }
}
