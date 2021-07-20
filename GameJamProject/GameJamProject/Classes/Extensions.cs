using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public static class Extensions
    {
        public static float GetAngle(Vector2 p1, Vector2 p2)
        {
            float xDiff = p1.X - p2.X;
            float yDiff = p1.Y - p2.Y;
            return -(float)Math.Atan2(yDiff, xDiff);
        }

        public static float AngleDifference(float a1, float a2)
        {
            float a = (a1 - a2) + (float)Math.PI * 4;
            return (float)(((a + Math.PI) % (Math.PI * 2)) - Math.PI);
        }
    }
}
