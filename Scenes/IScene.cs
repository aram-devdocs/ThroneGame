using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Maps;

namespace ThroneGame.Scenes
{
    public interface IScene
    {


        Game1 Game { get; set; }
        PlayerEntity Player { get; set; }
        CameraController CameraController { get; set; }
        PhysicsController PhysicsController { get; set; }
        RenderTarget2D MapRenderTarget { get; set; }
        Texture2D BackgroundImage { get; set; }
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);

        void Reset(Game1 game1, GameTime gameTime);

        IMap Map { get; set; }
        List<IEntity> Entities { get; set; }
    }
}