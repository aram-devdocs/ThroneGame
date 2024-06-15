using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThroneGame.Utils;

namespace ThroneGame.UI
{
    /// <summary>
    /// A class to calculate and display frames per second.
    /// </summary>
    public class FPSCounter
    {
        private int _frameCount;
        private double _elapsedTime;
        private double _fps;
        private SpriteFont _font;

        /// <summary>
        /// Initializes a new instance of the <see cref="FPSCounter"/> class.
        /// </summary>
        /// <param name="font">The font used to display the FPS.</param>
        public FPSCounter(SpriteFont font)
        {
            _font = font ?? throw new ArgumentNullException(nameof(font));
        }

        /// <summary>
        /// Updates the FPS counter.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += GameUtils.GetDeltaTime(gameTime);
            _frameCount++;

            if (_elapsedTime >= 1)
            {
                _fps = _frameCount / _elapsedTime;
                _frameCount = 0;
                _elapsedTime = 0;
            }
        }

        /// <summary>
        /// Draws the FPS counter on the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            string fpsText = $"FPS: {_fps:0.00}";
            spriteBatch.DrawString(_font, fpsText, new Vector2(50, 50), Color.Black);
        }
    }
}