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
        public int length { get { return _length; } set { value = Math.Min(value, 200);  _length = value; legPosition = Math.Max(1, value / 3 - 1); } }
        int _length = 6;
        int legPosition = 1;
        float segmentLength = 16 * Game1.pixelScale;
        int segmentListSize = 20;

        // Gameplay
        public bool started = false;
        public bool canStart = false;
        public bool alive = true;
        private float deadTime;
        private int deadSegments = 0;
        private float deathSpeed = 0;
        private float deadAlpha = 0;

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
            if (hitstop < 0.0001f && alive)
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

            if (canStart && Input.KeyPressed(Keys.Space))
            {
                started = true;
            }

            if (Input.KeyPressed(Keys.H))
                length = 50;

            // Death
            if (alive && started)
            {
                Rectangle bounds = new Rectangle((int)Camera.Location.X - Camera.Bounds.Width / 2, (int)Camera.Location.Y - Camera.Bounds.Height / 2, Camera.Bounds.Width, Camera.Bounds.Height);
                if (!bounds.Contains(Position))
                {
                    // Die
                    alive = false;
                    deadTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    deathSpeed = Math.Max(-0.01f * (length - 6) + 0.16f, 0.05f);
                }
            }
            if (!alive)
            {
                float deadTimeTotal = (float)gameTime.TotalGameTime.TotalSeconds - deadTime;
                deadAlpha = Math.Min(deadTimeTotal, 1);
                int deadSegmentsPrev = deadSegments;
                deadSegments = Math.Min((int)(deadTimeTotal / deathSpeed), length);
                if (deadSegments > deadSegmentsPrev)
                    SoundManager.PlaySoundEffect("hit", (float)Math.Pow(0.95f, deadSegments));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Fade
            if (!alive)
                SpriteManager.DrawSprite(spriteBatch, "SprPlaceholder", Camera.Location - Game1.gameInstance.viewSize.ToVector2() / 2, Color.White * deadAlpha, -300, 0, SpriteEffects.None, 100);

            // Head
            SpriteEffects flipped = SpriteEffects.None;
            if (Math.Abs(Extensions.AngleDifference(headAngle, (float)Math.PI)) < (float)Math.PI / 2)
                flipped = SpriteEffects.FlipVertically;
            string sprite = alive ? "SprDragonHead" : "SprDragonHeadSkeleton";
            SpriteManager.DrawSprite(spriteBatch, sprite, Position, Color.White, 0, headAngle, flipped);

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
                 sprite = "SprDragonBody";
                if (i == legPosition || i == 2 * legPosition + 1)
                    sprite = "SprDragonArm";
                if (i == length - 1)
                    sprite = "SprDragonTail";
                if (!alive && deadSegments > i)
                    sprite += "Skeleton";

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
