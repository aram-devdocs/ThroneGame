using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Tiles
{
    public class Tile : ITile
    {
        public Texture2D Texture { get; set; }
        public bool IsCollidable { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Tile(Texture2D texture, bool isCollidable, int width = 32, int height = 32)
        {
            Texture = texture;
            IsCollidable = isCollidable;
            Width = width;
            Height = height;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(Texture, new Rectangle(x, y, Width, Height), Color.White);
        }
    }
}