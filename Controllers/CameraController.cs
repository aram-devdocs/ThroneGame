using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the camera, adjusting its position based on the player's position and a specified padding.
    /// Ensures the camera does not show areas outside the map bounds.
    /// </summary>
    public class CameraController
    {
        private readonly Viewport _viewport;
        private Vector2 _position;
        private readonly float _padding;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly int _tileWidth;
        private readonly int _tileHeight;

        public bool IsDirty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraController"/> class.
        /// </summary>
        /// <param name="viewport">The viewport used to display the game.</param>
        /// <param name="padding">The padding around the edges of the viewport within which the camera will not move.</param>
        /// <param name="mapWidth">The width of the map in tiles.</param>
        /// <param name="mapHeight">The height of the map in tiles.</param>
        /// <param name="tileWidth">The width of a single tile.</param>
        /// <param name="tileHeight">The height of a single tile.</param>
        public CameraController(Viewport viewport, float padding, int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            if (padding < 0 || padding >= 0.5f)
            {
                throw new System.ArgumentOutOfRangeException(nameof(padding), "Padding must be between 0 and 0.49");
            }

            _viewport = viewport;
            _position = Vector2.Zero;
            _padding = padding;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            IsDirty = false;
        }

        /// <summary>
        /// Gets the view matrix for the camera, used to transform the world coordinates to screen coordinates.
        /// </summary>
        /// <returns>The view matrix for the camera.</returns>
        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }

        /// <summary>
        /// Updates the camera position based on the player's position.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="playerPosition">The current position of the player.</param>
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            IsDirty = false;
            UpdateHorizontalPosition(playerPosition);
            UpdateVerticalPosition(playerPosition);

            // Ensure the camera position stays within the map bounds
            _position.X = MathHelper.Clamp(_position.X, 0, _mapWidth * _tileWidth - _viewport.Width);
            _position.Y = MathHelper.Clamp(_position.Y, 0, _mapHeight * _tileHeight - _viewport.Height);
        }

        private void UpdateHorizontalPosition(Vector2 playerPosition)
        {
            float leftBound = _position.X + _viewport.Width * _padding;
            float rightBound = _position.X + _viewport.Width * (1 - _padding);

            if (playerPosition.X < leftBound)
            {
                _position.X = playerPosition.X - _viewport.Width * _padding;
                IsDirty = true;
            }
            else if (playerPosition.X > rightBound)
            {
                _position.X = playerPosition.X - _viewport.Width * (1 - _padding);
                IsDirty = true;
            }
        }

        private void UpdateVerticalPosition(Vector2 playerPosition)
        {
            float topBound = _position.Y + _viewport.Height * _padding;
            float bottomBound = _position.Y + _viewport.Height * (1 - _padding);

            if (playerPosition.Y < topBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * _padding;
                IsDirty = true;
            }
            else if (playerPosition.Y > bottomBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * (1 - _padding);
                IsDirty = true;
            }
        }

        /// <summary>
        /// Gets the visible area of the camera in world coordinates.
        /// </summary>
        /// <returns>The visible area of the camera in world coordinates.</returns>
        public Rectangle GetVisibleArea()
        {
            Matrix inverseViewMatrix = Matrix.Invert(GetViewMatrix());
            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            Vector2 bottomRight = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseViewMatrix);
            return new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)(bottomRight.X - topLeft.X), (int)(bottomRight.Y - topLeft.Y));
        }
    }
}