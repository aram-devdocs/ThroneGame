using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Maps;
using ThroneGame.Tiles;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {
        private Game1 _game;
        private List<ITile> _tiles;
        private PlayerEntity _player;
        private CameraController _cameraController;

        private PhysicsController _physicsController;

        private DemoMap _demoMap;

        public DemoScene(Game1 game)
        {
            _game = game;
            _tiles = new List<ITile>();
            _cameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            _physicsController = new PhysicsController();

        }

        public override void LoadContent()
        {
            BackgroundImage = _game.Content.Load<Texture2D>("Backgrounds/1");

            // Create and load the demo map
            _demoMap = new DemoMap("Content/Maps/DemoMap.json");
            _demoMap.LoadMapContent(_game.GraphicsDevice, _game.Content);

            // Load tile textures
            var tileTexture = _game.Content.Load<Texture2D>("Tiles/1");

            // Create tiles with positions
            _tiles.Add(new Tile(tileTexture, true, new Vector2(0, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(32, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(64, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(96, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(128, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(160, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(192, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(224, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(256, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(288, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(320, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(352, 350)));
            _tiles.Add(new Tile(tileTexture, true, new Vector2(384, 350)));
            // Add more tiles as needed

            // Load player texture and create player
            _player = new PlayerEntity(new Vector2(100, 0), _game.Content); // Assuming 6 frames for idle animation



            // Add tiles and player to physics controller
            foreach (var tile in _tiles)
            {
                _physicsController.AddTile(tile);
            }

            // Add map tiles to physics controller
            foreach (var tile in _demoMap.Tiles)
            {
                _physicsController.AddTile(tile);
            }


            _physicsController.AddEntity(_player);
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _physicsController.Update(gameTime);
            _cameraController.Update(gameTime, _player.Position);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            // Draw background
            spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            // Draw tiles and player with camera transformation
            spriteBatch.Begin(transformMatrix: _cameraController.GetViewMatrix());


            // TODO - do we need this anymore?
            // Draw tiles using multiple threads
            Parallel.ForEach(_tiles, tile =>
            {
                tile.Draw(spriteBatch, (int)tile.Position.X, (int)tile.Position.Y);
            });

            // Draw the map
            _demoMap.Draw(spriteBatch);

            // Draw player
            _player.Draw(spriteBatch);


        }
    }
}
