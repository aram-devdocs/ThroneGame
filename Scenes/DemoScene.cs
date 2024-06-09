using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Tiles;

namespace ThroneGame.Scenes
{
    public class DemoScene : Scene
    {
        private Game1 _game;
        private List<ITile> _tiles;
        private PlayerEntity _player;
        private MovementController _movementController;
        private PhysicsController _physicsController;
        private CameraController _cameraController;

        public DemoScene(Game1 game)
        {
            _game = game;
            _tiles = new List<ITile>();
            _movementController = new MovementController();
            _physicsController = new PhysicsController();
            _cameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
        }

        public override void LoadContent()
        {
            BackgroundImage = _game.Content.Load<Texture2D>("Backgrounds/1");

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
            var playerTexture = _game.Content.Load<Texture2D>("PlayerSprites/Shinobi/Idle"); // Example: using the Idle sprite sheet
            _player = new PlayerEntity(playerTexture, new Vector2(100, 100), playerTexture.Width / 6, playerTexture.Height, 6); // Assuming 6 frames for idle animation
        }

        public override void Update(GameTime gameTime)
        {
            _movementController.HandleMovement(_player, gameTime);
            _physicsController.ApplyPhysics(_player, _tiles, gameTime);
            _player.Update(gameTime);
            _cameraController.Update(gameTime, _player.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            // Draw background
            spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            // Draw tiles and player with camera transformation
            spriteBatch.Begin(transformMatrix: _cameraController.GetViewMatrix());

            // Draw tiles
            foreach (var tile in _tiles)
            {
                tile.Draw(spriteBatch, (int)tile.Position.X, (int)tile.Position.Y);
            }

            // Draw player
            _player.Draw(spriteBatch);


        }
    }
}
