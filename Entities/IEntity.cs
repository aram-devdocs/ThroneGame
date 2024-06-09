using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Entities
{
    public interface IEntity
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        bool IsCollidable { get; set; }
        bool IsOnGround { get; set; }
        int FrameWidth { get; }
        int FrameHeight { get; }
        bool IsFacingRight { get; set; }


        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}