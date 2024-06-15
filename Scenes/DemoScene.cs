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
            Map = new DemoMap(Game.Content);
            Player = new PlayerEntity(new Vector2(200, 300), Game.Content);
        }

    }
}