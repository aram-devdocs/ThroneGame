using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;

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

        bool IsFacingRight { get; set; } 

        Texture2D Texture { get; set; }
        Rectangle SourceRectangle { get; set; }
        string State { get; set; }

        MovementController MovementController { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}