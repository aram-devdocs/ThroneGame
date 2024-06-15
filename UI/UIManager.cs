using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThroneGame.Utils;


namespace ThroneGame.UI
{
    /// <summary>
    /// A class to calculate and display frames per second.
    /// </summary>


    public class UIManagerProps
    {
        public bool ShowFPS { get; set; }
    }

    public class UIManager
    {


        private FPSCounter _fpsCounter;






        public UIManager(Game1 game, UIManagerProps props)
        {
            if (props.ShowFPS)
            {
                var font = game.Content.Load<SpriteFont>("Fonts/Default");
                _fpsCounter = new FPSCounter(font);
            }
        }

        public void Update(GameTime gameTime)
        {
            _fpsCounter?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw UI on top of everything
            spriteBatch.Begin();
            _fpsCounter?.Draw(spriteBatch);
            spriteBatch.End();

        }

    }
}