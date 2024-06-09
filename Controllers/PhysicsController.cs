using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ThroneGame.Entities;
using ThroneGame.Tiles;

namespace ThroneGame.Controllers
{
    public class PhysicsController
    {
        private const float Gravity = 500f;

        public void ApplyPhysics(IEntity entity, List<ITile> tiles, GameTime gameTime)
        {
            // Apply gravity
            entity.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.IsOnGround = false;

            // Update entity position
            Vector2 newPosition = entity.Position + entity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for collisions
            foreach (var tile in tiles)
            {
                if (tile.IsCollidable && IsColliding(newPosition, entity, tile))
                {
                    // Adjust position and velocity based on collision
                    newPosition = HandleCollision(entity, tile);
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                    break;
                }
            }

            entity.Position = newPosition;
        }

        private bool IsColliding(Vector2 position, IEntity entity, ITile tile)
        {
            Rectangle entityRect = new Rectangle((int)position.X, (int)position.Y, entity.FrameWidth, entity.FrameHeight);
            Rectangle tileRect = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.Width, tile.Height);

            return entityRect.Intersects(tileRect);
        }

        private Vector2 HandleCollision(IEntity entity, ITile tile)
        {
            // Simple collision handling - place the entity on top of the tile
            return new Vector2(entity.Position.X, tile.Position.Y - entity.FrameHeight);
        }
    }
}