using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Tiles
{
    public interface ITile
    {
        Texture2D Texture { get; set; }
        bool IsCollidable { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        void Draw(SpriteBatch spriteBatch, int x, int y);
    }
}