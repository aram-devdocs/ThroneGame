using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Maps;

namespace ThroneGame.Scenes
{
    public abstract class Scene : IScene
    {



        public Game1 Game { get; set; }
        public PlayerEntity Player { get; set; }
        public CameraController CameraController { get; set; }
        public PhysicsController PhysicsController { get; set; }
        public RenderTarget2D MapRenderTarget { get; set; }
        public Texture2D BackgroundImage { get; set; }

        public List<IEntity> Entities { get; set; }
        public IMap Map { get; set; }
        private double _lastResetTime;

        public Scene(Game1 game)
        {
            Game = game;
            CameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            PhysicsController = new PhysicsController();

        }

        private void UpdateTiles()
        {

            this.Game.GraphicsDevice.SetRenderTarget(this.MapRenderTarget);
            this.Game.GraphicsDevice.Clear(Color.Transparent);
            using (var spriteBatch = new SpriteBatch(this.Game.GraphicsDevice))
            {
                spriteBatch.Begin();
                this.Map.Draw(spriteBatch);
                spriteBatch.End();
            }
            this.Game.GraphicsDevice.SetRenderTarget(null);


        }


        public virtual void LoadContent()
        {
            // Initialize render target
            this.MapRenderTarget = new RenderTarget2D(this.Game.GraphicsDevice, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height);


            this.Map.LoadContent(this.Game.GraphicsDevice, this.Game.Content);

            // Draw tiles to the render target
            this.Game.GraphicsDevice.SetRenderTarget(this.MapRenderTarget);
            this.Game.GraphicsDevice.Clear(Color.Transparent);
            using (var spriteBatch = new SpriteBatch(this.Game.GraphicsDevice))
            {
                spriteBatch.Begin();
                this.Map.Draw(spriteBatch);
                spriteBatch.End();
            }
            this.Game.GraphicsDevice.SetRenderTarget(null);






            // Add map tiles to physics controller
            foreach (var tile in this.Map.Tiles)
            {
                this.PhysicsController.AddTile(tile);
            }

            this.PhysicsController.AddEntity(this.Player);


        }
        public virtual void Update(GameTime gameTime)
        {
            this.Player.Update(gameTime);
            this.PhysicsController.Update(gameTime);
            this.CameraController.Update(gameTime, this.Player.Position);

            UpdateTiles();

        }
        public virtual void Reset(Game1 game1, GameTime gameTime)

        {
            // Load the content again if the last reset was more than 1 second ago
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastResetTime > 1000)
            {
                LoadContent();
                game1.ResetElapsedTime();
                _lastResetTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {




            // Draw background
            spriteBatch.Begin();
            if (BackgroundImage != null)
            {
                // Draw the background image to fit the screen size
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            }


            spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();




            // TODO - Why are we beginning and ending the spritebatch multiple times? It only works this way to draw the background each time always in the viewport, but there must be a better way to do this.
            // Draw tiles and player with camera transformation
            spriteBatch.Begin(transformMatrix: this.CameraController.GetViewMatrix());
            // Draw the map
            spriteBatch.Draw(this.MapRenderTarget, Vector2.Zero, Color.White);
            // Draw player
            this.Player.Draw(spriteBatch);

            // Debug physics controller
            this.PhysicsController.Draw(spriteBatch);

            spriteBatch.End();

        }
    }
}