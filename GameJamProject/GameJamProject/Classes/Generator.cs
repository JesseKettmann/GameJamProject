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
        static float difficulty;
        static List<Object> objects;

        public static void Initialise()
        {
            random = new Random();
            ground = 172 * Game1.pixelScale;
        }
        
        public static float SpawnOutpost(float _cursor, List<Object> _objects, float _difficulty)
        {
            // Variables
            cursor = _cursor;
            cursorHeight = 0;
            objects = _objects;
            difficulty = _difficulty;

            // Decide on outposttype
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 0.16f) // 10% chance of a single tower
            {
                SpawnTower();
            } else // outpost
            {
                float startCursor = cursor + 30 * Game1.pixelScale;
                if (random.NextDouble() < 0.5f)
                    SpawnGate();
                if (random.NextDouble() < 0.5f)
                    SpawnTower();
                List<int> structures = new List<int>();
                int buildings = random.Next(1, 4); // Buildings
                for (int i = 0; i < buildings; i++)
                    structures.Add(0);
                int carts = random.Next(0, 3); // Carts
                for (int i = 0; i < carts; i ++)
                    structures.Add(1);
                int towers = random.Next(0, (int)(difficulty * 3)); // Towers
                for (int i = 0; i < towers; i++)
                    structures.Add(2);
                if (random.NextDouble() < 0.5f * (0.5 + 0.5 * difficulty)) // Temple
                    structures.Add(3);
                Shuffle(structures);
                SpawnList(structures);
                if (random.NextDouble() < 0.5f)
                    SpawnTower();

                // Spawn samurai
                int samurai = (int)((cursor - startCursor) * 0.003f);
                for (int i = 0; i < samurai; i ++)
                    objects.Add(new Samurai(new Vector2(startCursor + (cursor - startCursor) * (float)random.NextDouble(), ground), 2 * i));
            }

            // Return new outpostposition
            return cursor + 900 + 900 * (float)random.NextDouble();
        }

        private static void Shuffle(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int helper = list[i];
                int randomLoc = random.Next(0, list.Count);
                list[i] = list[randomLoc];
                list[randomLoc] = helper;
            }
        }

        private static void SpawnTower()
        {
            // Base
            SpawnTowerSection(new Vector2(cursor, ground), "SprTowerBottom");

            // Tower height
            cursorHeight = -32 * Game1.pixelScale;
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
            objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprRoofSide"));
            objects.Add(new Building(new Vector2(cursor + 16 * Game1.pixelScale, ground + cursorHeight), "SprRoofMid"));
            objects.Add(new Building(new Vector2(cursor + 32 * Game1.pixelScale, ground + cursorHeight), "SprRoofSide", true));

            // Chance of barrel
            cursor += 24 * Game1.pixelScale;
            int barrels = random.Next(3);
            for (int i = 0; i < barrels; i ++)
                SpawnBarrel(new Vector2(cursor + random.Next(-26, 27) * Game1.pixelScale, ground), i * 2);

            cursor += 26 * Game1.pixelScale;
        }

        private static void SpawnTowerSection(Vector2 position, string sprite)
        {
            objects.Add(new Building(position, sprite));
            if (random.NextDouble() < 0.5f)
                SpawnLampion(position + new Vector2(16.5f * Game1.pixelScale, -28 * Game1.pixelScale));
            if (random.NextDouble() < 0.5f * difficulty)
                objects.Add(new Archer(position + new Vector2(17 * Game1.pixelScale, 0), 0));
        }

        private static void SpawnLampion(Vector2 position)
        {
            objects.Add(new Destructible(position, "SprLampion", 2, -9850));
        }

        private static void SpawnBarrel(Vector2 position, int depthAdd)
        {
            objects.Add(new Destructible(position, "SprBarrel", 3, -9825 + depthAdd));
        }

        private static void SpawnGate()
        {
            objects.Add(new Destructible(new Vector2(cursor, ground), "SprGate", 6, -9820));
            cursor += 48 * Game1.pixelScale;
        }

        private static void SpawnBuilding()
        {
            // Building type
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 0.3f) // 30% chance of long building
            {
                SpawnTempleSide();
                int width = random.Next(4, 10);
                for (int i = 0; i < width; i++)
                    SpawnTempleMid();
                SpawnTempleSide(true);
                cursor += 8 * Game1.pixelScale;
            } else if (randomNumber < 0.6f) // 30% chance of symmetrical building
            {
                SpawnTempleSide();
                int width = random.Next(1, 3);
                for (int i = 0; i < width; i++)
                    SpawnTempleMid();

                SpawnBlock();

                cursor += 7 * Game1.pixelScale;
                cursorHeight = -50 * Game1.pixelScale;
                SpawnTowerSection(new Vector2(cursor, ground + cursorHeight), "SprTowerMid");
                cursorHeight -= 32 * Game1.pixelScale;
                cursor -= 7 * Game1.pixelScale;
                objects.Add(new Building(new Vector2(cursor, ground + cursorHeight), "SprRoofSide"));
                objects.Add(new Building(new Vector2(cursor + 16 * Game1.pixelScale, ground + cursorHeight), "SprRoofMid"));
                objects.Add(new Building(new Vector2(cursor + 32 * Game1.pixelScale, ground + cursorHeight), "SprRoofSide", true));
                cursor += 48 * Game1.pixelScale;

                for (int i = 0; i < width; i++)
                    SpawnTempleMid();
                SpawnTempleSide(true);
                cursor += 8 * Game1.pixelScale;
            }
        }

        private static void SpawnBlock()
        {
            objects.Add(new Building(new Vector2(cursor, ground), "SprBlock"));
        }

        private static void SpawnTempleSide(bool flip = false)
        {
            objects.Add(new Building(new Vector2(cursor, ground), "SprTempleSide", flip));
            objects.Add(new Building(new Vector2(cursor, ground - 32 * Game1.pixelScale), "SprRoofSide", flip));
            cursor += 16 * Game1.pixelScale;
        }

        private static void SpawnTempleMid()
        {
            objects.Add(new Building(new Vector2(cursor, ground), "SprTempleMid"));
            objects.Add(new Building(new Vector2(cursor, ground - 32 * Game1.pixelScale), "SprRoofMid"));
            randomNumber = (float)random.NextDouble();
            if (randomNumber < 0.35f)
                SpawnLampion(new Vector2(cursor + 8 * Game1.pixelScale, ground - 32 * Game1.pixelScale));
            else if (randomNumber < 0.5f)
                SpawnBarrel(new Vector2(cursor + 8 * Game1.pixelScale, ground - 6 * Game1.pixelScale), 0);
            cursor += 16 * Game1.pixelScale;
        }

        private static void SpawnCart(int depthAdd)
        {
            objects.Add(new Destructible(new Vector2(cursor, ground), "SprCart", 6, -9810 + depthAdd, (random.NextDouble() > 0.5f)));
            cursor += 28 * Game1.pixelScale;
        }

        private static void SpawnStatue()
        {
            objects.Add(new Destructible(new Vector2(cursor, ground), "SprStatue", 10, -9805));
        }

        private static void SpawnTemple()
        {
            SpawnStatue();
            SpawnBlock();
            float groundPrev = ground;
            ground -= 50 * Game1.pixelScale;
            SpawnTempleSide();
            if (random.NextDouble() < 2 * difficulty)
                objects.Add(new Archer(new Vector2(cursor + 8 * Game1.pixelScale, ground), 0));
            SpawnTempleMid();
            SpawnTempleSide(true);
            ground = groundPrev;
            SpawnStatue();

            cursor += 8 * Game1.pixelScale;
        }

        private static void SpawnList(List<int> list)
        {
            for (int i = 0; i < list.Count; i ++)
            {
                switch (list[i])
                {
                    case 0: SpawnBuilding(); break;
                    case 1: SpawnCart(i); break;
                    case 2: SpawnTower(); break;
                    case 3: SpawnTemple(); break;
                }
            }
        }
    }
}