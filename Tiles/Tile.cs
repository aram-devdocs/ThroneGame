using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Tiles
{
    /// <summary>
    /// Represents a tile in the game.
    /// </summary>
    public class Tile : ITile
    {
        /// <summary>
        /// Gets or sets the position of the tile.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is collidable.
        /// </summary>
        public bool IsCollidable { get; set; }

        /// <summary>
        /// Gets or sets the width of the tile.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the tile.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the bounds of the tile.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the tile. (Not used as tiles are static)
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the mass of the tile. (Not used as tiles are static)
        /// </summary>
        public float Mass { get; set; }


        private readonly Texture2D _texture;
        private readonly Rectangle _sourceRectangle;



        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class with the specified parameters.
        /// </summary>
        /// <param name="texture">The texture of the tile.</param>
        /// <param name="sourceRectangle">The source rectangle of the tile in the texture.</param>
        /// <param name="isCollidable">Whether the tile is collidable.</param>
        /// <param name="position">The position of the tile.</param>
        /// <param name="width">The width of the tile.</param>
        /// <param name="height">The height of the tile.</param>
        public Tile(Texture2D texture, Rectangle sourceRectangle, bool isCollidable, Vector2 position, int width, int height)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle;
            IsCollidable = isCollidable;
            Position = position;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Updates the tile's state. (Not used as tiles are static)
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {
            // Do nothing as tiles are static
        }

        /// <summary>
        /// Draws the tile using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font = null)
        {
            spriteBatch.Draw(_texture, Position, _sourceRectangle, Color.White);
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Destroys the tile. This method should be overridden by derived classes to implement destruction logic.
        /// </summary>
        public void Destroy()
        {
            // TODO: Implement destruction logic
        }
    }
}