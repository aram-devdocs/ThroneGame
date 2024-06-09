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
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _sceneManager = new SceneManager();
            var demoScene = new DemoScene(this); // Pass only the Game1 instance
            _sceneManager.AddScene("DemoScene", demoScene);
            _sceneManager.SetCurrentScene("DemoScene");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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