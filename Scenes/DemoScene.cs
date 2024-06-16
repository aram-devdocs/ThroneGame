using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Entities;
using ThroneGame.Maps;
using ThroneGame.UI;

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
            Player = new PlayerEntity(new Vector2(200, 200), Game.Content, game);
            UIManagerProps uiManagerProps = new UIManagerProps
            {
                ShowFPS = true
            };
            UIManager = new UIManager(game, uiManagerProps);

            Entities.Add(new EnemyEntity(new Vector2(400, 300), Game.Content, game));
            Entities.Add(new EnemyEntity(new Vector2(600, 300), Game.Content, game));
            Entities.Add(new EnemyEntity(new Vector2(800, 300), Game.Content, game));
            Entities.Add(new EnemyEntity(new Vector2(1000, 300), Game.Content, game));
            Entities.Add(new EnemyEntity(new Vector2(1200, 300), Game.Content, game));

        }

    }
}