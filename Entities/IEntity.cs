using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Entities
{
    public interface IEntity : IGameObject
    {
        Vector2 Velocity { get; set; }
        bool IsOnGround { get; set; }
        int FrameWidth { get; }
        int FrameHeight { get; }
        bool IsFacingRight { get; set; }
    }
}