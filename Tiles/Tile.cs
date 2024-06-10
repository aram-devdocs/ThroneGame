using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Utils;

namespace ThroneGame.Tiles
{
    public class Tile : ITile
    {
        public Vector2 Position { get; set; }
        public bool IsCollidable { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private Texture2D _texture;
        private Rectangle _sourceRectangle;

        public Tile(Texture2D texture, bool isCollidable, Vector2 position, int width = 64, int height = 64)
        {
            _texture = texture;
            IsCollidable = isCollidable;
            Position = position;
            Width = width;
            Height = height;
            _sourceRectangle = new Rectangle(0, 0, width, height);
        }

        public Tile(Texture2D texture, Rectangle sourceRectangle, bool isCollidable, Vector2 position, int width, int height)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle;
            IsCollidable = isCollidable; // Assuming collidable for now
            Position = position;
            Width = width;
            Height = height;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(_texture, new Rectangle(x, y, Width, Height), _sourceRectangle, Color.White);

        }
    }
}