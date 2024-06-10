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
            List<ITile> nearbyTiles = GetNearbyTiles(entity);
            foreach (var tile in nearbyTiles)
            {
                if (tile.IsCollidable && IsColliding(newPosition, entity, tile))
                {
                    // Adjust position and velocity based on collision
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }
            }

            // Apply Gravity
            if (!entity.IsOnGround)
            {
                entity.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            entity.Position = newPosition;
        }

        private bool IsColliding(Vector2 position, IEntity entity, ITile tile)
        {
            // Use the bounds from entities and tiles to check for collision
            Rectangle entityBounds = entity.Bounds;
            Rectangle tileBounds = tile.Bounds;

            return entityBounds.Intersects(tileBounds);
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
            Point topLeftCell = GetCell(entity.Position);
            Point bottomRightCell = GetCell(entity.Position + new Vector2(entity.FrameWidth, entity.FrameHeight));

            // Use threading for parallel processing
            Parallel.For(topLeftCell.X - 1, bottomRightCell.X + 2, x =>
            {
                for (int y = topLeftCell.Y - 1; y <= bottomRightCell.Y + 1; y++)
                {
                    Point cell = new Point(x, y);
                    if (_tileGrid.ContainsKey(cell))
                    {
                        lock (nearbyTiles)
                        {
                            nearbyTiles.AddRange(_tileGrid[cell]);
                        }
                    }
                }
            });

            return nearbyTiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            bool debug = true;

            if (debug)
            {

                foreach (var entity in _entities)
                {
                    TextureUtils.DebugBorder(spriteBatch, (int)entity.Position.X, (int)entity.Position.Y, entity.FrameWidth, entity.FrameHeight);
                    List<ITile> nearbyTiles = GetNearbyTiles(entity);
                    foreach (var tile in nearbyTiles)
                    {
                        TextureUtils.DebugBorder(spriteBatch, (int)tile.Position.X, (int)tile.Position.Y, tile.Width, tile.Height);
                    }
                }
            }

        }
    }
}