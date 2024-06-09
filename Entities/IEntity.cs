using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Tiles;

namespace ThroneGame.Entities
{
    public interface IEntity
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        bool IsCollidable { get; set; }
        bool IsOnGround { get; set; }
        int FrameWidth { get; set; }
        int FrameHeight { get; set; }
        Texture2D Texture { get; set; }
        Rectangle SourceRectangle { get; set; }
        string State { get; set; }

        MovementController MovementController { get; set; }
        PhysicsController PhysicsController { get; set; }

        void Update(GameTime gameTime, List<ITile> tiles);
        void Draw(SpriteBatch spriteBatch);
    }
}