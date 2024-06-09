using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {
        private Game1 _game;

        public DemoScene(Game1 game)
        {
            _game = game;
        }

        public override void LoadContent()
        {
            BackgroundImage = _game.Content.Load<Texture2D>("Backgrounds/1");
        }

        public override void Update(GameTime gameTime)
        {
            // Update logic for the scene
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Additional drawing logic for the scene, if any
        }
    }
}