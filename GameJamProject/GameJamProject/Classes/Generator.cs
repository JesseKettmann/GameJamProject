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
            SpawnTowerSection(new Vector2(cursor, ground), "SprTowerBottom");

            // Tower height
            cursorHeight -= 32 * Game1.pixelScale;
            SpawnTowerSection(new Vector2(cursor, ground + cursorHeight), "SprTowerMid");
            cursorHeight -= 32 * Game1.pixelScale;
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 0.70f) // 70% chance of at least 3 stories
            {
                SpawnTowerSection(new Vector2(cursor, ground + cursorHeight), "SprTowerMid");
                cursorHeight -= 32 * Game1.pixelScale;
                if (randomNumber < 0.30f) // 30% chance of 4 stories
                {
                    SpawnTowerSection(new Vector2(cursor, ground + cursorHeight), "SprTowerMid");
                    cursorHeight -= 32 * Game1.pixelScale;
                }
            }

            // Roof
            cursor -= 7 * Game1.pixelScale;
            objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprRoofSide", true));
            objects.Add(new Building(new Vector2(cursor + 16 * Game1.pixelScale, ground + cursorHeight), "SprRoofMid"));
            objects.Add(new Building(new Vector2(cursor + 32 * Game1.pixelScale, ground + cursorHeight), "SprRoofSide"));

            // Chance of barrel
            cursor += 24 * Game1.pixelScale;
            int barrels = random.Next(3);
            for (int i = 0; i < barrels; i ++)
            {
                objects.Add(new Destructible(new Vector2(cursor + random.Next(-26, 27) * Game1.pixelScale, ground), "SprBarrel", 3, -9498 + i * 2));
            }
        }

        private static void SpawnTowerSection(Vector2 position, string sprite)
        {
            objects.Add(new Building(position, sprite));
            if (random.NextDouble() < 0.5f)
                objects.Add(new Destructible(position + new Vector2(16.5f * Game1.pixelScale, -28 * Game1.pixelScale), "SprLampion", 2, -9499));
        }
    }
}