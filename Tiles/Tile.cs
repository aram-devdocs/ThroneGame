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
        public Rectangle Bounds { get; set; }

        public float HorizontalBoundsPadding { get; set; }
        public float VerticalBoundsPadding { get; set; }

        public Tile(Texture2D texture, bool isCollidable, Vector2 position, int width = 64, int height = 64)
        {
            _texture = texture;
            IsCollidable = isCollidable;
            Position = position;
            Width = width;
            Height = height;
            _sourceRectangle = new Rectangle(0, 0, width, height);
            HorizontalBoundsPadding = 1;
            VerticalBoundsPadding = 1;
            // Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            Bounds = new Rectangle((int)(Position.X * HorizontalBoundsPadding), (int)(Position.Y * VerticalBoundsPadding), Width, Height);
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

        public void Update(GameTime gameTime)
        {
            // Do nothing as tiles are static
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(_texture, new Rectangle(x, y, Width, Height), _sourceRectangle, Color.White);
            spriteBatch.Draw(_texture, Position, _sourceRectangle, Color.White);
            // TODO - Clean up vertical bounds padding
            Bounds = new Rectangle((int)(Position.X - (HorizontalBoundsPadding / 2)), (int)(Position.Y - (VerticalBoundsPadding / 2)), (int)(Width + HorizontalBoundsPadding), (int)(Height + VerticalBoundsPadding));

        }

        public void Destroy()
        {
            // TODO: Implement
        }
    }
}