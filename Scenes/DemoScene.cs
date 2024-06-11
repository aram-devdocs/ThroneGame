using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Entities;
using ThroneGame.Maps;

namespace ThroneGame.Scenes
{
    /// <summary>
    /// Represents a demo scene in the game.
    /// </summary>
    public class DemoScene : Scene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoScene"/> class with the specified game instance.
        /// </summary>
        /// <param name="game">The game instance associated with this scene.</param>
        public DemoScene(Game1 game) : base(game)
        {
            LoadBackgroundImage();
            CreateDemoMap();
            CreatePlayer();
        }

        /// <summary>
        /// Loads the background image for the demo scene.
        /// </summary>
        private void LoadBackgroundImage()
        {
            BackgroundImage = Game.Content.Load<Texture2D>("Backgrounds/1");
        }

        /// <summary>
        /// Creates and loads the demo map for the scene.
        /// </summary>
        private void CreateDemoMap()
        {
            Map = new DemoMap(Game.Content);
        }

        /// <summary>
        /// Creates the player entity for the demo scene.
        /// </summary>
        private void CreatePlayer()
        {
            Player = new PlayerEntity(new Vector2(300, 200), Game.Content);
        }
    }
}