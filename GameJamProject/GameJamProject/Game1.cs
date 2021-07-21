using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    public class Game1 : Game
    {
        public static Game1 gameInstance;
        public Gamestate gamestate;
        public static Matrix cameraMatrix;
        public static Random random;

        // Rendering
        private SpriteBatch _spriteBatch;
        public Point viewSize = new Point(1366, 768);
        public Point portSize = new Point(1366, 768);
        //public Point portSize = new Point(1920, 1080);
        public float viewScale = 1f;
        private GraphicsDeviceManager _graphics;
        public static float pixelScale = 4f;
        public RenderTarget2D renderTarget;
        public static GraphicsDevice graphicsDevice;

        // Colors
        public Color White = new Color(214, 214, 206);
        public Color Orange = new Color(61, 88, 21);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Global
            gameInstance = this;
            random = new Random();
        }

        protected override void Initialize()
        {
            // Rendering
            _graphics.PreferredBackBufferWidth = portSize.X;
            _graphics.PreferredBackBufferHeight = portSize.Y;
            _graphics.IsFullScreen = false; // Start in fullscreen mode
            _graphics.ApplyChanges();
            viewScale = 1f / ((float)viewSize.Y / portSize.Y);

            base.Initialize();

            HighscoreManager.RetrieveHighscore();
        }

        protected override void LoadContent()
        {

            // Rendering
            graphicsDevice = GraphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            renderTarget = new RenderTarget2D(graphicsDevice, viewSize.X, viewSize.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            SpriteManager.Initialise();
            SoundManager.Initialise();

            // Textures
            SpriteManager.AddTexture("TexDragon", "chinese_dragon");

            SpriteManager.AddTexture("TexDragonSkeleton", "chinese_dragon_skeleton");

            SpriteManager.AddTexture("TexSky", "skybox");

            SpriteManager.AddTexture("TexGround", "ground");

            SpriteManager.AddTexture("TexBuildings", "buildings");

            // Sprites
            SpriteManager.AddSprite("SprDragonHead", "TexDragon", new Vector2(8, 5.5f), new Rectangle(48, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonBody", "TexDragon", new Vector2(16, 5.5f), new Rectangle(16, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonArm", "TexDragon", new Vector2(16, 5.5f), new Rectangle(32, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonTail", "TexDragon", new Vector2(16, 5.5f), new Rectangle(0, 0, 16, 11));

            SpriteManager.AddSprite("SprDragonHeadSkeleton", "TexDragonSkeleton", new Vector2(8, 5.5f), new Rectangle(48, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonBodySkeleton", "TexDragonSkeleton", new Vector2(16, 5.5f), new Rectangle(16, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonArmSkeleton", "TexDragonSkeleton", new Vector2(16, 5.5f), new Rectangle(32, 0, 16, 11));
            SpriteManager.AddSprite("SprDragonTailSkeleton", "TexDragonSkeleton", new Vector2(16, 5.5f), new Rectangle(0, 0, 16, 11));

            SpriteManager.AddSprite("SprSky", "TexSky", Vector2.Zero);

            SpriteManager.AddSprite("SprGround", "TexGround", Vector2.Zero);

            SpriteManager.AddSprite("SprTempleSide", "TexBuildings", new Vector2(0, 32), new Rectangle(0, 32, 16, 32));
            SpriteManager.AddSprite("SprTempleMid", "TexBuildings", new Vector2(0, 32), new Rectangle(16, 32, 16, 32));
            SpriteManager.AddSprite("SprRoofSide", "TexBuildings", new Vector2(0, 16), new Rectangle(0, 16, 16, 16));
            SpriteManager.AddSprite("SprRoofMid", "TexBuildings", new Vector2(0, 16), new Rectangle(16, 16, 16, 16));
            SpriteManager.AddSprite("SprBlock", "TexBuildings", new Vector2(0, 50), new Rectangle(32, 14, 48, 50));
            SpriteManager.AddSprite("SprTowerBottom", "TexBuildings", new Vector2(0, 32), new Rectangle(119, 32, 34, 32));
            SpriteManager.AddSprite("SprTowerMid", "TexBuildings", new Vector2(0, 32), new Rectangle(119, 0, 34, 32));

            SpriteManager.AddSprite("SprLampion", "TexBuildings", new Vector2(5.5f, 0), new Rectangle(82, 32, 11, 12));
            SpriteManager.AddSprite("SprBarrel", "TexBuildings", new Vector2(8, 16), new Rectangle(96, 48, 16, 16));
            SpriteManager.AddSprite("SprCart", "TexBuildings", new Vector2(5, 37), new Rectangle(165, 27, 39, 37));
            SpriteManager.AddSprite("SprStatue", "TexBuildings", new Vector2(11, 41), new Rectangle(213, 23, 22, 41));
            SpriteManager.AddSprite("SprGate", "TexBuildings", new Vector2(0, 48), new Rectangle(240, 16, 48, 48));

            SpriteManager.AddSprite("SprPlaceholder", "TexBuildings", new Vector2(0, 0), new Rectangle(288, 42, 16, 22));

            SpriteManager.AddFont("BigFont", "BigFont");
            SpriteManager.AddFont("MediumFont", "MediumFont");

            SoundManager.LoadSoundEffect("hit", "Audio/hit");
            SoundManager.LoadSoundEffect("wind", "Audio/Wind");


            gamestate = new Level();
        }

        protected override void Update(GameTime gameTime)
        {
            #region remove later

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #endregion

            // Get the current inputs
            Input.Update();

            // Update game
            gamestate.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the backboard
            GraphicsDevice.Clear(Orange);

            // Draw everything to the render target
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Orange);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: cameraMatrix, samplerState: SamplerState.PointClamp);
            gamestate.Draw(_spriteBatch);
            _spriteBatch.End();


            //Draw gamestate UI
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            gamestate.DrawUI(_spriteBatch);
            _spriteBatch.End();

            // Reset render target
            graphicsDevice.SetRenderTarget(null);

            // Draw the render target to the screen
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, viewScale, SpriteEffects.None, 0f);
            _spriteBatch.End();

            // XNA
            base.Draw(gameTime);
        }
    }
}
