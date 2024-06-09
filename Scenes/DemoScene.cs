using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {
        private Texture2D texture;

        public DemoScene(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            // Initialize scene-specific components here
        }

        public override void LoadContent()
        {
            // Load scene-specific content here
            texture = game.Content.Load<Texture2D>("Background/1"); // Example texture
        }

        public override void Update(GameTime gameTime)
        {
            // Update scene-specific logic here
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }
    }
}