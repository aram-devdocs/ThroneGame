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

        public UIManager UIManager;
        public PhysicsController PhysicsController { get; set; }
        public List<ITile> VisibleTiles { get; set; }
        public List<IEntity> Entities { get; set; }
        public IMap Map { get; set; }

        private SpriteFont defaultFont;

        private double _lastResetTime;

        public Scene(Game1 game)
        {
            Game = game;
            CameraController = new CameraController(game.GraphicsDevice.Viewport, 0.49f);
            PhysicsController = new PhysicsController();
            Entities = new List<IEntity>();
            VisibleTiles = new List<ITile>();
            UIManagerProps uiManagerProps = new UIManagerProps
            {
                ShowFPS = true
            };
            UIManager = new UIManager(game, uiManagerProps);

            defaultFont = game.Content.Load<SpriteFont>("Fonts/Default");
        }

        public virtual void LoadContent()
        {
            PhysicsController.LoadMap(Map);
            PhysicsController.AddEntity(Player);
            Entities.ForEach(entity => PhysicsController.AddEntity(entity));
            Map.LoadContent(Game.GraphicsDevice, Game.Content);
            Map.DrawToRenderTarget(Game.GraphicsDevice, new SpriteBatch(Game.GraphicsDevice));

        }

        public virtual void Update(GameTime gameTime)
        {


            Player.Update(gameTime);
            Entities.ForEach(entity => entity.Update(gameTime));
            PhysicsController.Update(gameTime);
            CameraController.Update(gameTime, Player.Position);
            UIManager.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {


            Map.DrawBackground(spriteBatch);

            spriteBatch.Begin(transformMatrix: CameraController.GetViewMatrix());

            Map.DrawTileMap(spriteBatch);
            Entities.ForEach(entity => entity.Draw(spriteBatch, defaultFont));
            Player.Draw(spriteBatch, defaultFont);
            PhysicsController.Draw(spriteBatch);


            spriteBatch.End();

            UIManager.Draw(spriteBatch);




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