using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThroneGame.Entities;
using ThroneGame.Tiles;
using ThroneGame.Utils;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the physics of entities in the game, including applying gravity and handling collisions.
    /// </summary>
    public class PhysicsController
    {
        private const float Gravity = 500f;
        private const int CellSize = 32; // Size of each cell in the grid for spatial partitioning

        private readonly List<IEntity> _entities;
        private readonly Dictionary<Point, List<ITile>> _tileGrid;

        public PhysicsController()
        {
            _entities = new List<IEntity>();
            _tileGrid = new Dictionary<Point, List<ITile>>();
        }

        /// <summary>
        /// Adds an entity to be managed by the physics controller.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Adds a tile to the spatial grid for collision detection.
        /// </summary>
        /// <param name="tile">The tile to add.</param>
        public void AddTile(ITile tile)
        {
            Point cell = GetCell(tile.Position);
            if (!_tileGrid.ContainsKey(cell))
            {
                _tileGrid[cell] = new List<ITile>();
            }
            _tileGrid[cell].Add(tile);
        }

        /// <summary>
        /// Updates the physics for all entities.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last update.</param>
        public void Update(GameTime gameTime)
        {
            Parallel.ForEach(_entities, entity =>
            {
                ApplyPhysics(entity, gameTime);
            });
        }

        /// <summary>
        /// Draws debug borders around entities and nearby tiles if debug mode is enabled.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            bool debug = true;

            if (debug)
            {
                foreach (var entity in _entities)
                {
                    TextureUtils.DebugBorder(spriteBatch, entity.Bounds.X, entity.Bounds.Y, entity.Bounds.Width, entity.Bounds.Height);
                    List<ITile> nearbyTiles = GetNearbyTiles(entity);
                    foreach (var tile in nearbyTiles)
                    {
                        TextureUtils.DebugBorder(spriteBatch, tile.Bounds.X, tile.Bounds.Y, tile.Bounds.Width, tile.Bounds.Height);
                    }
                }
            }
        }

        /// <summary>
        /// Applies physics to a single entity, including gravity and collision detection.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="gameTime">Time elapsed since the last update.</param>
        private void ApplyPhysics(IEntity entity, GameTime gameTime)
        {
            Vector2 newPosition = entity.Position + entity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            List<ITile> nearbyTiles = GetNearbyTiles(entity);

            // Handle collisions with tiles
            Parallel.ForEach(nearbyTiles, tile =>
            {
                if (tile.IsCollidable)
                {
                    HandleCollision(entity, ref newPosition, tile);
                }
            });

            if (!entity.IsOnGround)
            {
                entity.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            entity.Position = newPosition;
        }

        /// <summary>
        /// Checks if an entity is colliding with a tile.
        /// </summary>
        /// <param name="entityBounds">The bounds of the entity.</param>
        /// <param name="tileBounds">The bounds of the tile.</param>
        /// <returns>True if there is a collision, false otherwise.</returns>
        private bool IsColliding(Rectangle entityBounds, Rectangle tileBounds)
        {
            return entityBounds.Intersects(tileBounds);
        }

        /// <summary>
        /// Adjusts an entity's position and velocity based on collision with a tile.
        /// </summary>
        /// <param name="entity">The entity to adjust.</param>
        /// <param name="newPosition">The new position of the entity.</param>
        /// <param name="tile">The tile that the entity collided with.</param>
        private void HandleCollision(IEntity entity, ref Vector2 newPosition, ITile tile)
        {
            Rectangle entityBounds = entity.Bounds;
            Rectangle tileBounds = tile.Bounds;

            if (IsColliding(entityBounds, tileBounds))
            {
                Rectangle intersection = Rectangle.Intersect(entityBounds, tileBounds);

                if (intersection.Width < intersection.Height)
                {
                    // Collision on left or right
                    if (entityBounds.Center.X < tileBounds.Center.X)
                    {
                        // Collision on the left
                        newPosition.X = tileBounds.Left - entityBounds.Width - 1; // Add buffer of 1 pixel
                    }
                    else
                    {
                        // Collision on the right
                        newPosition.X = tileBounds.Right + 1; // Add buffer of 1 pixel
                    }
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
                else
                {
                    // Collision on top or bottom
                    if (entityBounds.Center.Y < tileBounds.Center.Y)
                    {
                        // Collision on top
                        newPosition.Y = tileBounds.Top - entityBounds.Height;
                        entity.IsOnGround = true;
                    }
                    else
                    {
                        // Collision on the bottom
                        newPosition.Y = tileBounds.Bottom;
                    }
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
            }
        }

        /// <summary>
        /// Converts a position to a cell coordinate in the spatial grid.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <returns>The cell coordinate.</returns>
        private Point GetCell(Vector2 position)
        {
            int cellX = (int)(position.X / CellSize);
            int cellY = (int)(position.Y / CellSize);
            return new Point(cellX, cellY);
        }

        /// <summary>
        /// Gets the tiles near an entity for collision detection.
        /// </summary>
        /// <param name="entity">The entity to find nearby tiles for.</param>
        /// <returns>A list of nearby tiles.</returns>
        private List<ITile> GetNearbyTiles(IEntity entity)
        {
            List<ITile> nearbyTiles = new List<ITile>();
            Point topLeftCell = GetCell(entity.Position);
            Point bottomRightCell = GetCell(entity.Position + new Vector2(entity.FrameWidth, entity.FrameHeight));

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
    }
}