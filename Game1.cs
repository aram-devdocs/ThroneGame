using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Scenes;

namespace ThroneGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneManager _sceneManager;

        public static GraphicsDeviceManager Graphics;

        public IScene currentScene => _sceneManager._currentScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics = _graphics;
        }

        protected override void Initialize()
        {
            // Initialize the screen size here
            _graphics.PreferredBackBufferWidth = 1280; // Set the desired width
            _graphics.PreferredBackBufferHeight = 720; // Set the desired height
            _graphics.SynchronizeWithVerticalRetrace = false; // Disable VSync to achieve 120 FPS
            _graphics.ApplyChanges();

            // Set the desired frame rate to 120 FPS
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _sceneManager = new SceneManager();
            var demoScene = new DemoScene(this); // Pass only the Game1 instance
            var homeScene = new HomeScene(this);
            _sceneManager.AddScene("DemoScene", demoScene);
            _sceneManager.AddScene("HomeScene", homeScene);
            _sceneManager.SetCurrentScene("DemoScene");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _sceneManager.ResetCurrentScene(this, gameTime, _spriteBatch);
            }
            _sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sceneManager.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}