using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public static class Camera
    {
        private static float cameraSpeed = 1f;
        public static Vector2 Location;

        public static Rectangle Bounds { get; set; }
        public static float targetX;

        private static Random random;

        private static float shakeTime = 0.0f;
        private static float shakeDuration = 0.0f;
        private static float shakeMagnitude = 0.0f;
        private static Vector2 shakeOffset = Vector2.Zero;
        public static Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0f)) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0)) * 
                    Matrix.CreateTranslation(new Vector3(shakeOffset, 0));
            }
        }

        public static void Initialise(Point viewport, Vector2 position)
        {
            random = new Random();
            Bounds = new Rectangle(0, 0, viewport.X, viewport.Y);
            Location = position;
            targetX = position.X;
        }

        public static void Update(GameTime gameTime)
        {
            Location.X = MathHelper.Lerp(Location.X, targetX, cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            Location = Vector2.Round(Location);
            
            
            if(shakeTime > 0.0f)
            {
                float damp = 1.0f;

                shakeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                float percentage = (shakeDuration - shakeTime) / shakeDuration;
                if(percentage <= 1.0f)
                {
                    damp = 1.0f - percentage;   
                }

                Vector2 offset = RandomInUnitCircle();
                offset *= shakeMagnitude * damp;
                shakeOffset = offset;

            } else
            {
                shakeTime = 0;
                shakeOffset = Vector2.Zero;
            }


            Game1.cameraMatrix = TransformMatrix;



        }

        /*
         * https://github.com/JordanKisiel/UnityCameraShake/blob/master/CameraShake.cs
         */

        public static void Shake(float magnitude, float duration, float dampStartPercentage)
        {
            dampStartPercentage = MathHelper.Clamp(dampStartPercentage, 0f, 1f);
            shakeTime = duration;
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }

        private static Vector2 RandomInUnitCircle()
        {
            double a = random.NextDouble() * (2 * Math.PI) - Math.PI;
            float x = MathF.Cos((float)a);
            float y = MathF.Sin((float)a);
            return new Vector2(x, y);
        }
        

    }
}
