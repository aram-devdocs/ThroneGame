using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Maps;
using ThroneGame.Tiles;
using ThroneGame.UI;

namespace ThroneGame.Scenes
{
    /// <summary>
    /// Represents an abstract base class for a game scene.
    /// </summary>
    public abstract class Scene : IScene
    {
        /// <summary>
        /// Gets or sets the game instance associated with this scene.
        /// </summary>
        public Game1 Game { get; set; }

        /// <summary>
        /// Gets or sets the player entity in the scene.
        /// </summary>
        public PlayerEntity Player { get; set; }

        /// <summary>
        /// Gets or sets the camera controller for the scene.
        /// </summary>
        public CameraController CameraController { get; set; }

        /// <summary>
        /// Gets or sets the physics controller for the scene.
        /// </summary>
        public PhysicsController PhysicsController { get; set; }

        /// <summary>
        /// Gets or sets the render target for the map.
        /// </summary>

        /// <summary>
        /// Gets or sets the background image for the scene.
        /// </summary>
        public Texture2D BackgroundImage { get; set; }


        /// <summary>
        /// Gets or sets the list of tiles in the scene.
        /// </summary>
        public List<ITile> VisibleTiles { get; set; }

        /// <summary>
        /// Gets or sets the list of entities in the scene.
        /// </summary>
        public List<IEntity> Entities { get; set; }

        /// <summary>
        /// Gets or sets the map of the scene.
        /// </summary>
        public IMap Map { get; set; }


        private RenderTarget2D _mapRenderTarget;

        private double _lastResetTime;
        private FPSCounter _fpsCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class with the specified game instance.
        /// </summary>
        /// <param name="game">The game instance associated with this scene.</param>
        public Scene(Game1 game)
        {
            Game = game;
            CameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            PhysicsController = new PhysicsController();
            Entities = new List<IEntity>();
            VisibleTiles = new List<ITile>();
            _mapRenderTarget = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Loads the content required for the scene.
        /// </summary>
        public virtual void LoadContent()
        {

            Map.LoadContent(Game.GraphicsDevice, Game.Content);

            // Initialize the render target with a size that can fit the entire map
            int mapWidth = Map.MapWidth * Map.TileWidth;
            int mapHeight = Map.MapHeight * Map.TileHeight;


            _mapRenderTarget = new RenderTarget2D(Game.GraphicsDevice, mapWidth, mapHeight);

            // Load map content

            // Draw tiles to the render target
            Game.GraphicsDevice.SetRenderTarget(_mapRenderTarget);
            Game.GraphicsDevice.Clear(Color.Transparent);
            var spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin();
            foreach (var tile in Map.Tiles)
            {
                tile.Draw(spriteBatch);
                PhysicsController.AddTile(tile);
            }
            spriteBatch.End();
            Game.GraphicsDevice.SetRenderTarget(null);

            // Add player to physics controller
            PhysicsController.AddEntity(Player);

            // Load FPS counter font
            var font = Game.Content.Load<SpriteFont>("Fonts/Default"); // Ensure you have an appropriate SpriteFont in your content
            _fpsCounter = new FPSCounter(font);
        }

        /// <summary>
        /// Updates the scene based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public virtual void Update(GameTime gameTime)
        {
            PhysicsController.Update(gameTime);
            Player.Update(gameTime);
            CameraController.Update(gameTime, Player.Position);
            _fpsCounter.Update(gameTime);
        }


        /// <summary>
        /// Draws the scene using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Begin();
            DrawBackground(spriteBatch);
            spriteBatch.End();

            // Draw tiles and player with camera transformation
            spriteBatch.Begin(transformMatrix: CameraController.GetViewMatrix());



            // Draw the map render target
            spriteBatch.Draw(_mapRenderTarget, Vector2.Zero, Color.White);




            Player.Draw(spriteBatch);

            // Debug physics controller
            PhysicsController.Draw(spriteBatch);

            // Draw FPS counter
            _fpsCounter.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Resets the scene to its initial state.
        /// </summary>
        /// <param name="game1">The game instance.</param>
        /// <param name="gameTime">The game time information.</param>
        public virtual void Reset(Game1 game1, GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Load the content again if the last reset was more than 1 second ago
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastResetTime > 1000)
            {

                Player.Position = new Vector2(100, 300);

                game1.ResetElapsedTime();
                _lastResetTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }

        /// <summary>
        /// Draws the background image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            if (BackgroundImage != null)
            {
                // Draw the background image to fit the screen size
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            }
        }


        /// <summary>
        /// Deprecated: Updates the list of visible tiles in the scene. This method is no longer used as we are using a RenderTarget2D to draw the map. If we run into performance issues, we can consider using this method again.
        /// </summary>
        public void DeprecatedUpdateVisibleTiles()
        {
            var visibleArea = CameraController.GetVisibleArea();
            VisibleTiles = Map.Tiles.Where(tile => tile.Bounds.Intersects(visibleArea)).ToList();
        }
    }
}