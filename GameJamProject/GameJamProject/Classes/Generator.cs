using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public static class Generator
    {
        static Random random;
        static float ground;
        static float randomNumber;
        static float cursor;
        static float cursorHeight = 0;
        static List<Object> objects;

        public static void Initialise()
        {
            random = new Random();
            ground = 172 * Game1.pixelScale;
        }
        
        public static float SpawnOutpost(float _cursor, List<Object> _objects)
        {
            // Variables
            cursor = _cursor;
            cursorHeight = 0;
            objects = _objects;

            // Decide on outposttype
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 1) // 10% chance of a single tower
            {
                SpawnTower();
            }

            // Return new outpostposition
            return cursor + 1000 + 800 * (float)random.NextDouble();
        }

        private static void SpawnTower()
        {
            // Base
            objects.Add(new Building(new Vector2(cursor, ground), "SprTowerBottom"));

            // Tower height
            cursorHeight -= 32 * Game1.pixelScale;
            objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprTowerMid"));
            cursorHeight -= 32 * Game1.pixelScale;
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 0.70f) // 70% chance of at least 3 stories
            {
                objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprTowerMid"));
                cursorHeight -= 32 * Game1.pixelScale;
                if (randomNumber < 0.30f) // 30% chance of 4 stories
                {
                    objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprTowerMid"));
                    cursorHeight -= 32 * Game1.pixelScale;
                }
            }
            cursor -= 7 * Game1.pixelScale;
            objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprRoofSide", true));
            objects.Add(new Building(new Vector2(cursor + 16 * Game1.pixelScale, ground + cursorHeight), "SprRoofMid"));
            objects.Add(new Building(new Vector2(cursor + 32 * Game1.pixelScale, ground + cursorHeight), "SprRoofSide"));
        }
    }
}
