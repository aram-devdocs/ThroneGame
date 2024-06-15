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
    public abstract class Scene : IScene
    {
        public Game1 Game { get; set; }
        public PlayerEntity Player { get; set; }
        public CameraController CameraController { get; set; }
        public PhysicsController PhysicsController { get; set; }
        public List<ITile> VisibleTiles { get; set; }
        public List<IEntity> Entities { get; set; }
        public IMap Map { get; set; }

        private double _lastResetTime;
        private FPSCounter _fpsCounter;

        public Scene(Game1 game)
        {
            Game = game;
            CameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            PhysicsController = new PhysicsController();
            Entities = new List<IEntity>();
            VisibleTiles = new List<ITile>();
        }

        public virtual void LoadContent()
        {
            PhysicsController.LoadMap(Map);
            PhysicsController.AddEntity(Player);
            Map.LoadContent(Game.GraphicsDevice, Game.Content);
            Map.DrawToRenderTarget(Game.GraphicsDevice, new SpriteBatch(Game.GraphicsDevice));
            var font = Game.Content.Load<SpriteFont>("Fonts/Default");
            _fpsCounter = new FPSCounter(font);
        }

        public virtual void Update(GameTime gameTime)
        {


            Player.Update(gameTime);
            // TODO- should physics controller be called last?
            PhysicsController.Update(gameTime);
            CameraController.Update(gameTime, Player.Position);
            _fpsCounter.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {


            Map.DrawBackground(spriteBatch);

            spriteBatch.Begin(transformMatrix: CameraController.GetViewMatrix());

            Map.DrawTileMap(spriteBatch);
            Player.Draw(spriteBatch);
            PhysicsController.Draw(spriteBatch);

            _fpsCounter.Draw(spriteBatch);

            spriteBatch.End();
        }

        public virtual void Reset(Game1 game1, GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastResetTime > 1000)
            {
                Player.Position = new Vector2(100, 300);
                game1.ResetElapsedTime();
                _lastResetTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }



    }
}