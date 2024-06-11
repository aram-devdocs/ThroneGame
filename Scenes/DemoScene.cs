using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Entities;
using ThroneGame.Maps;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {

        public DemoScene(Game1 game) : base(game)
        {
            this.BackgroundImage = game.Content.Load<Texture2D>("Backgrounds/1");
            // Create and load the demo map
            this.Map = new DemoMap(
                this.Game.Content
            );


            // Load player texture and create player
            this.Player = new PlayerEntity(new Vector2(300, 200), this.Game.Content);

        }

    }
}
