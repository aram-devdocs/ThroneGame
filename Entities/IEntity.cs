using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Entities
{
    public interface IEntity : IGameObject
    {
        bool IsOnGround { get; set; }
        int FrameWidth { get; }
        int FrameHeight { get; }
        bool IsFacingRight { get; set; }

    }
}