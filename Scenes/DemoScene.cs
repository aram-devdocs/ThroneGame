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


        private RenderTarget2D _mapRenderTarget;
        private DemoMap _demoMap;

        private bool _tilesDirty = true; // Flag to indicate if tiles need to be redrawn


        public DemoScene(Game1 game)
        {
            _game = game;
            _tiles = new List<ITile>();
            _cameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            _physicsController = new PhysicsController();

        }

        public void UpdateTiles()
        {
            if (_tilesDirty)
            {
                _game.GraphicsDevice.SetRenderTarget(_mapRenderTarget);
                _game.GraphicsDevice.Clear(Color.Transparent);
                using (var spriteBatch = new SpriteBatch(_game.GraphicsDevice))
                {
                    spriteBatch.Begin();
                    _demoMap.Draw(spriteBatch);
                    spriteBatch.End();
                }
                _game.GraphicsDevice.SetRenderTarget(null);

                _tilesDirty = false; // Reset the dirty flag
            }
        }




        public override void LoadContent()
        {
            BackgroundImage = _game.Content.Load<Texture2D>("Backgrounds/1");


            // Initialize render target
            _mapRenderTarget = new RenderTarget2D(_game.GraphicsDevice, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);

            // Create and load the demo map
            _demoMap = new DemoMap("Content/Maps/DemoMap.json");
            _demoMap.LoadMapContent(_game.GraphicsDevice, _game.Content);

            // Draw tiles to the render target
            _game.GraphicsDevice.SetRenderTarget(_mapRenderTarget);
            _game.GraphicsDevice.Clear(Color.Transparent);
            using (var spriteBatch = new SpriteBatch(_game.GraphicsDevice))
            {
                spriteBatch.Begin();
                _demoMap.Draw(spriteBatch);
                spriteBatch.End();
            }
            _game.GraphicsDevice.SetRenderTarget(null);



            // Load player texture and create player
            _player = new PlayerEntity(new Vector2(300, 200), _game.Content);



            // Add tiles in map and player to physics controller. TODO: Add entities to physics controller

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

            UpdateTiles();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {




            // Draw background
            spriteBatch.Begin();

            spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();




            // TODO - Why are we beginning and ending the spritebatch multiple times? It only works this way to draw the background each time always in the viewport, but there must be a better way to do this.
            // Draw tiles and player with camera transformation
            spriteBatch.Begin(transformMatrix: _cameraController.GetViewMatrix());
            // Draw the map
            spriteBatch.Draw(_mapRenderTarget, Vector2.Zero, Color.White);
            // Draw player
            _player.Draw(spriteBatch);

            spriteBatch.End();


        }
    }
}
