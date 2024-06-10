using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Tiles
{
    public interface ITile : IGameObject
    {
        int Width { get; set; }
        int Height { get; set; }

    }
}