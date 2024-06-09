using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public abstract class Scene : IScene
    {
        public Texture2D BackgroundImage { get; set; }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        public virtual void Reset(Game1 game1)
        {
            // Load the content again
            LoadContent();
            game1.ResetElapsedTime();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (BackgroundImage != null)
            {
                // Draw the background image to fit the screen size
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            }
        }
    }
}