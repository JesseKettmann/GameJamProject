using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public float deadTime;
        private int deadSegments = 0;
        private float deathSpeed = 0;
        private float deadAlpha = 0;
        private float startX = 0;

        private double startTime = -1f;

        float startY;
        public float hitstop = 0;

        SoundEffectInstance windSound;


        public Dragon(Vector2 position) : base(position)
        {
            SegmentPositions = new List<Vector2>();
            startY = position.Y;

            windSound = SoundManager.PlaySoundEffectInstance("wind");
            windSound.IsLooped = true;
            windSound.Volume = 0;
            windSound.Play();
        }

        public override void Update(GameTime gameTime)
        {
            if(startTime == -1)
                startTime = gameTime.TotalGameTime.TotalSeconds;
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
                    //System.Diagnostics.Debug.WriteLine(Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10));
                    direction = Math.Sign(Math.Cos((gameTime.TotalGameTime.TotalSeconds-startTime)*10) + (Position.Y - startY) / 200);
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

            if (canStart && Input.KeyPressed(Keys.Space) && started == false)
            {
                started = true;
                startX = Position.X;
            }

            // Get arrows
            Level level = Game1.gameInstance.gamestate as Level;
            if (level == null)
                return;
            List<Arrow> arrows = new List<Arrow>();
            foreach (Object obj in level.objects)
                if (obj as Arrow != null)
                    arrows.Add(obj as Arrow);

            // Difficulty
            level.difficulty = started ? MathHelper.Clamp(((Position.X - startX) / 27000) + 0.4f, 0.5f, 2f) : 0.5f;

            bool died = false;
            if (alive && started)
            {
                // Death by walls
                Rectangle bounds = new Rectangle((int)Camera.Location.X - Camera.Bounds.Width / 2, (int)Camera.Location.Y - Camera.Bounds.Height / 2, Camera.Bounds.Width, Camera.Bounds.Height);
                if (!bounds.Contains(Position))
                {
                    died = true;
                }

                // Death by arrow
                Rectangle headBound = new Rectangle((int)(Position.X - 2 * Game1.pixelScale), (int)(Position.Y - 2 * Game1.pixelScale), (int)(4 * Game1.pixelScale), (int)(4 * Game1.pixelScale));
                for (int o = arrows.Count - 1; o >= 0; o--)
                {
                    Rectangle arrowBound = new Rectangle((int)(arrows[o].Position.X - 1), (int)(arrows[o].Position.Y - 1), 2, 2);
                    if (headBound.Intersects(arrowBound) && !arrows[o].frozen)
                    {
                        died = true;
                        arrows[o].depth = 50;
                        arrows[o].frozen = true;
                        break;
                    }
                }

                // Death by self
                for (int i = 5; i < SegmentPositions.Count; i++)
                {
                    Rectangle segmentBound = new Rectangle((int)(SegmentPositions[i].X - 2 * Game1.pixelScale), (int)(SegmentPositions[i].Y - 2 * Game1.pixelScale), (int)(4 * Game1.pixelScale), (int)(4 * Game1.pixelScale));
                    if (headBound.Intersects(segmentBound))
                    {
                        died = true;
                        break;
                    }
                }

                // Block arrows
                for (int i = 5; i < SegmentPositions.Count; i++)
                {
                    Rectangle segmentBound = new Rectangle((int)(SegmentPositions[i].X - 2 * Game1.pixelScale), (int)(SegmentPositions[i].Y - 2 * Game1.pixelScale), (int)(4 * Game1.pixelScale), (int)(4 * Game1.pixelScale));
                    for (int o = arrows.Count - 1; o >= 0; o--)
                    {
                        Rectangle arrowBound = new Rectangle((int)(arrows[o].Position.X - 1), (int)(arrows[o].Position.Y - 1), 2, 2);
                        if (segmentBound.Intersects(arrowBound) && !arrows[o].frozen)
                        {
                            level.objects.Remove(arrows[o]);
                            arrows.RemoveAt(o);
                            SoundManager.PlaySoundEffect("block");
                        }
                    }
                }

                if (died)
                {
                    // Die
                    SoundManager.PlaySoundEffect("death", 0.45f);
                    alive = false;
                    deadTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    deathSpeed = Math.Max(-0.01f * (length - 6) + 0.14f, 0.07f);
                    Camera.Shake(8, 0.2f, 1);
                }
            }
            if (!alive)
            {
                float deadTimeTotal = (float)gameTime.TotalGameTime.TotalSeconds - deadTime;
                deadAlpha = Math.Min(deadTimeTotal, 1);
                int deadSegmentsPrev = deadSegments;
                deadSegments = Math.Min((int)(deadTimeTotal / deathSpeed), length);
                if (deadSegments > deadSegmentsPrev)
                    SoundManager.PlaySoundEffect("bone", (float)Math.Pow(0.95f, deadSegments) * 0.5f);
                windSound.Volume = MathHelper.Lerp(windSound.Volume, 0, (float)gameTime.ElapsedGameTime.TotalSeconds * 3f);
            
            
            } else {
                if (Input.KeyDown(Keys.Space))
                    windSound.Volume = MathHelper.Lerp(windSound.Volume, 1f, (float)gameTime.ElapsedGameTime.TotalSeconds * 3f);
                else
                    windSound.Volume = MathHelper.Lerp(windSound.Volume, 0.3f, (float)gameTime.ElapsedGameTime.TotalSeconds * 3f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Fade
            if (!alive)
                SpriteManager.DrawSprite(spriteBatch, "SprPlaceholder", Camera.Location - Game1.gameInstance.viewSize.ToVector2() / 2, Game1.gameInstance.Orange * deadAlpha, -300, 0, SpriteEffects.None, 100);

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
            try
            {
                thisPoint = SegmentPositions[o];
                Vector2 nextPoint;
                float angle = headAngle;
                for (int i = 0; i < length; i++)
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
                    }
                    catch
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
            catch { return; }

        }
    }
}
