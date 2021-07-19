using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public class Game1 : Game
    {
        public static Game1 gameInstance;
        private Gamestate gamestate;

        // Rendering
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Point viewSize = new Point(1920, 1080);
        public Point portSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        public float viewScale = 1f;
        public static GraphicsDevice graphicsDevice;
        public RenderTarget2D renderTarget;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameInstance = this;

            // Rendering
            IsMouseVisible = true;
            graphicsDevice = GraphicsDevice;
        }

        protected override void LoadContent()
        {
            gamestate = new Menu();

            // Rendering
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(graphicsDevice, viewSize.X, viewSize.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
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
            GraphicsDevice.Clear(Color.Black);

            // Draw everything to the render target
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: cameraMatrix, samplerState: SamplerState.PointClamp);
            gamestate.Draw(_spriteBatch);
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
