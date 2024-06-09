using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public abstract class Scene : IScene
    {
        public Texture2D BackgroundImage { get; set; }
        private double _lastResetTime;

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        public virtual void Reset(Game1 game1, GameTime gameTime)

        {
            // Load the content again if the last reset was more than 1 second ago
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastResetTime > 1000)
            {
                LoadContent();
                game1.ResetElapsedTime();
                _lastResetTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

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