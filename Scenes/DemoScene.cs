using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Tiles;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {
        private Game1 _game;
        private List<ITile> _tiles;

        public DemoScene(Game1 game)
        {
            _game = game;
            _tiles = new List<ITile>();
        }

        public override void LoadContent()
        {
            BackgroundImage = _game.Content.Load<Texture2D>("Backgrounds/1");

            // Load tile textures
            var tileTexture = _game.Content.Load<Texture2D>("Tiles/1");

            // Create tiles
            _tiles.Add(new Tile(tileTexture, true));
            // Add more tiles as needed
        }

        public override void Update(GameTime gameTime)
        {
            // Update logic for the scene
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw tiles
            foreach (var tile in _tiles)
            {
                // Example: drawing tiles at fixed positions (0, 0), (32, 0), etc.
                tile.Draw(spriteBatch, 0, 0);
                tile.Draw(spriteBatch, 32, 0);
                // Add more tile positions as needed
            }
        }
    }
}