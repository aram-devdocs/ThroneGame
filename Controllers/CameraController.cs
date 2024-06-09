using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Controllers
{
    public class CameraController
    {
        private readonly Viewport _viewport;
        private Vector2 _position;

        public CameraController(Viewport viewport)
        {
            _viewport = viewport;
            _position = Vector2.Zero;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0));
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float cameraWidth = _viewport.Width * 0.6f; // 60% of the viewport width
            float cameraHeight = _viewport.Height * 0.6f; // 60% of the viewport height
            float leftBound = _position.X + _viewport.Width * 0.2f; // 20% padding
            float rightBound = _position.X + _viewport.Width * 0.8f;
            float topBound = _position.Y + _viewport.Height * 0.2f;
            float bottomBound = _position.Y + _viewport.Height * 0.8f;

            if (playerPosition.X < leftBound)
            {
                _position.X = playerPosition.X - _viewport.Width * 0.2f;
            }
            else if (playerPosition.X > rightBound)
            {
                _position.X = playerPosition.X - _viewport.Width * 0.8f;
            }

            if (playerPosition.Y < topBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * 0.2f;
            }
            else if (playerPosition.Y > bottomBound)
            {
                _position.Y = playerPosition.Y - _viewport.Height * 0.8f;
            }
        }
    }
}