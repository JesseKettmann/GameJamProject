using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Dragon : Object
    {
        // Custom
        float turnSpeed = 0.005f;
        float moveSpeed = 0.6f;

        // Movement
        int direction = 0;
        float headAngle = 0;

        // Segments
        List<Vector2> SegmentPositions;
        public int length { get { return _length; } set { _length = value; legPosition = Math.Max(1, value / 3 - 1); } }
        int _length = 6;
        int legPosition = 1;
        float segmentLength = 16 * Game1.pixelScale;
        int segmentListSize = 20;

        // Gameplay
        public bool started = false;
        float startY;
        public float hitstop = 0;

        public Dragon(Vector2 position) : base(position)
        {
            SegmentPositions = new List<Vector2>();
            startY = position.Y;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (hitstop < 0.0001f)
            {
                // Movement
                if (started)
                {
                    direction = Input.KeyDown(Keys.Space) ? 1 : -1;
                }
                else
                {
                    direction = Math.Sign(Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10) + (Position.Y - startY) / 200);
                }
                headAngle += direction * turnSpeed * deltaTime;
                if (headAngle < 0)
                    headAngle += (float)Math.PI * 2;
                Position += new Vector2((float)Math.Cos(headAngle) * moveSpeed, -(float)Math.Sin(headAngle) * moveSpeed) * deltaTime;

                // Segments
                SegmentPositions.Insert(0, Position);
                if (SegmentPositions.Count > segmentListSize)
                    SegmentPositions.RemoveAt(SegmentPositions.Count - 1);
            }
            else hitstop -= deltaTime;

            // Start
            if (Input.KeyDown(Keys.Space))
                started = true;

            // Test
            if (Input.KeyPressed(Keys.H))
                length++;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Head
            SpriteEffects flipped = SpriteEffects.None;
            if (Math.Abs(Extensions.AngleDifference(headAngle, (float)Math.PI)) < (float)Math.PI / 2)
                flipped = SpriteEffects.FlipVertically;
            SpriteManager.DrawSprite(spriteBatch, "SprDragonHead", Position, Color.White, 0, headAngle, flipped);

            // Body
            Vector2 thisPoint = Position;
            int o = 0;
            try
            {
                while (Vector2.Distance(thisPoint, SegmentPositions[o]) < segmentLength / 2)
                    o++;
            } catch
            {
                o--;
            }
            o--;
            thisPoint = SegmentPositions[o];
            Vector2 nextPoint;
            float angle = headAngle;
            for (int i = 0; i < length; i ++)
            {
                // Set sprite
                string sprite = "SprDragonBody";
                if (i == legPosition || i == 2 * legPosition + 1)
                    sprite = "SprDragonArm";
                if (i == length - 1)
                    sprite = "SprDragonTail";

                // Find next point
                try
                {
                    while (Vector2.Distance(thisPoint, SegmentPositions[o]) < segmentLength)
                        o++;
                    o--;
                    nextPoint = SegmentPositions[o];
                    angle = Extensions.GetAngle(thisPoint, nextPoint);
                } catch
                {
                    nextPoint = new Vector2(0);
                    segmentListSize++;
                    sprite = "SprDragonTail";
                    i = length;
                }
                flipped = SpriteEffects.None;
                if (Math.Abs(Extensions.AngleDifference(angle, (float)Math.PI)) < (float)Math.PI / 2)
                    flipped = SpriteEffects.FlipVertically;
                SpriteManager.DrawSprite(spriteBatch, sprite, thisPoint, Color.White, -i, angle, flipped);
                thisPoint = nextPoint;
            }
        }
    }
}
