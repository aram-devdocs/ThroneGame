using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThroneGame.Entities;
using ThroneGame.Tiles;
using ThroneGame.Utils;

namespace ThroneGame.Controllers
{
    public class PhysicsController
    {
        private const float Gravity = 500f;
        private const int CellSize = 32; // Adjusted cell size for optimization

        private List<IEntity> _entities;
        private Dictionary<Point, List<ITile>> _tileGrid;

        public PhysicsController()
        {
            _entities = new List<IEntity>();
            _tileGrid = new Dictionary<Point, List<ITile>>();
        }

        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddTile(ITile tile)
        {
            Point cell = GetCell(tile.Position);
            if (!_tileGrid.ContainsKey(cell))
            {
                _tileGrid[cell] = new List<ITile>();
            }
            _tileGrid[cell].Add(tile);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                ApplyPhysics(entity, gameTime);
            }
        }


        private void ApplyPhysics(IEntity entity, GameTime gameTime)
        {
            // Set Is On Ground
            entity.IsOnGround = false;

            // Update entity position
            Vector2 newPosition = entity.Position + entity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for collisions
            // List<ITile> nearbyTiles = GetNearbyTiles(entity);

            Parallel.ForEach(_tileGrid, kvp =>
            {
                foreach (var tile in kvp.Value)
                {
                    if (tile.IsCollidable && IsColliding(newPosition, entity, tile))
                    {
                        // Adjust position and velocity based on collision
                        entity.Velocity = new Vector2(entity.Velocity.X, 0);
                        entity.IsOnGround = true;
                    }
                }
            });

            // Apply Gravity
            if (!entity.IsOnGround) entity.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;


            entity.Position = newPosition;
        }

        private bool IsColliding(Vector2 position, IEntity entity, ITile tile)
        {
            Rectangle entityRect = new Rectangle((int)position.X, (int)position.Y, entity.FrameWidth, entity.FrameHeight);
            Rectangle tileRect = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.Width, tile.Height);

            return entityRect.Intersects(tileRect);
        }
        private Point GetCell(Vector2 position)
        {
            int cellX = (int)(position.X / CellSize);
            int cellY = (int)(position.Y / CellSize);
            return new Point(cellX, cellY);
        }

        private List<ITile> GetNearbyTiles(IEntity entity)
        {
            List<ITile> nearbyTiles = new List<ITile>();
            Point entityCell = GetCell(entity.Position);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point cell = new Point(entityCell.X + x, entityCell.Y + y);
                    if (_tileGrid.ContainsKey(cell))
                    {
                        nearbyTiles.AddRange(_tileGrid[cell]);
                    }
                }
            }
            return nearbyTiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in _entities)
            {
                TextureUtils.DebugBorder(spriteBatch, (int)entity.Position.X, (int)entity.Position.Y, entity.FrameWidth, entity.FrameHeight);
            }

            // Draw collision boxes for tiles
            foreach (var kvp in _tileGrid)
            {
                foreach (var tile in kvp.Value)
                {
                    TextureUtils.DebugBorder(spriteBatch, (int)tile.Position.X, (int)tile.Position.Y, tile.Width, tile.Height);
                }
            }
        }
    }
}