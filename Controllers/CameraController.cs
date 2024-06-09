using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Controllers
{
    public class CameraController
    {
        private readonly Viewport _viewport;
        private Vector2 _position;
        private float _padding;

        public CameraController(Viewport viewport, float padding = 0.2f)
        {

            // Padding cannot be less than 0 or greater than .49
            if (padding < 0 || padding >= 0.5f)
            {
                throw new System.ArgumentOutOfRangeException("Padding must be between 0 and 0.49");
            }

            
            _viewport = viewport;
            _position = Vector2.Zero;
            _padding = padding;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float leftBound = _position.X + _viewport.Width * _padding;
            float rightBound = _position.X + _viewport.Width * (1 - _padding);
            float topBound = _position.Y + _viewport.Height * _padding;
            float bottomBound = _position.Y + _viewport.Height * (1 - _padding);

            if (playerPosition.X < leftBound)
            {
                _position.X = playerPosition.X - _viewport.Width * _padding;
            }
            else if (playerPosition.X > rightBound)
            {
                _position.X = playerPosition.X - _viewport.Width * (1 - _padding);
            }

            if (playerPosition.Y < topBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * _padding;
            }
            else if (playerPosition.Y > bottomBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * (1 - _padding);
            }
        }
    }
}