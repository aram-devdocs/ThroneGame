using System.Collections.Generic;
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
        }

        /// <summary>
        /// Loads the content required for the scene.
        /// </summary>
        public virtual void LoadContent()
        {

            // Load map content
            Map.LoadContent(Game.GraphicsDevice, Game.Content);

            // Draw tiles to the render target

            // Add map tiles to physics controller
            foreach (var tile in Map.Tiles)
            {
                PhysicsController.AddTile(tile);
            }

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
            // spriteBatch.Draw(MapRenderTarget, Vector2.Zero, Color.White);

            // Draw all the tiles in the map instead of the render target
            // Map.Tiles.ForEach(tile => tile.Draw(spriteBatch));


            // Get tiles in the visible area
            // var visibleTiles = Map.Tiles.Where(tile => tile.Bounds.Intersects(CameraController.GetVisibleArea()));
            // set visible tiles to the tiles in the scene



            if (CameraController.IsDirty)
            {
                UpdateVisibleTiles();
            }
            // Draw the tiles in the visible area
            foreach (var tile in VisibleTiles)
            {
                tile.Draw(spriteBatch);
            }




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
        /// Updates the list of visible tiles in the scene.
        /// </summary>
        public void UpdateVisibleTiles()
        {
            var visibleArea = CameraController.GetVisibleArea();
            VisibleTiles = Map.Tiles.Where(tile => tile.Bounds.Intersects(visibleArea)).ToList();
        }
    }
}