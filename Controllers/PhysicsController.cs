using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThroneGame.Entities;
using ThroneGame.Maps;
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
        private readonly List<IEntity> _entities;
        private IMap _map;

        // Concurrent dictionary for thread-safe caching of tile positions
        private readonly ConcurrentDictionary<Vector2, ITile> _tileCache;

        public PhysicsController()
        {
            _entities = new List<IEntity>();
            _tileCache = new ConcurrentDictionary<Vector2, ITile>();
        }

        public void LoadMap(IMap map)
        {
            _map = map;
            _tileCache.Clear(); // Clear the cache when a new map is loaded
        }

        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Updates the physics for all entities.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last update.</param>
        public void Update(GameTime gameTime)
        {
            float deltaTime = GameUtils.GetDeltaTime(gameTime);

            Parallel.ForEach(_entities, entity =>
            {
                if (IsEntityWithinBounds(entity))
                {
                    if (!entity.IsOnGround)
                    {
                        // Apply gravity
                        entity.Velocity = new Vector2(entity.Velocity.X, entity.Velocity.Y + Gravity * deltaTime);
                    }
                }
                else
                {
                    entity.Velocity = Vector2.Zero; // Stop movement if out of bounds
                }

                // Define positions to check for collisions
                Vector2 bottomCenter = new Vector2(entity.Bounds.X + entity.Bounds.Width / 2, entity.Bounds.Bottom);
                Vector2 topCenter = new Vector2(entity.Bounds.X + entity.Bounds.Width / 2, entity.Bounds.Top);
                Vector2 leftCenter = new Vector2(entity.Bounds.Left, entity.Bounds.Y + entity.Bounds.Height / 2);
                Vector2 rightCenter = new Vector2(entity.Bounds.Right, entity.Bounds.Y + entity.Bounds.Height / 2);

                // Create a list to store the tasks
                List<Task> tasks = new List<Task>();

                // Create tasks to get tiles from cache or map
                Task<ITile> bottomTileTask = Task.Run(() => GetTileFromCacheOrMap(bottomCenter));
                Task<ITile> topTileTask = Task.Run(() => GetTileFromCacheOrMap(topCenter));
                Task<ITile> leftTileTask = Task.Run(() => GetTileFromCacheOrMap(leftCenter));
                Task<ITile> rightTileTask = Task.Run(() => GetTileFromCacheOrMap(rightCenter));

                // Add tasks to the list
                tasks.Add(bottomTileTask);
                tasks.Add(topTileTask);
                tasks.Add(leftTileTask);
                tasks.Add(rightTileTask);

                // Wait for all tasks to complete
                Task.WaitAll(tasks.ToArray());

                // Get the results from the tasks
                ITile bottomTile = bottomTileTask.Result;
                ITile topTile = topTileTask.Result;
                ITile leftTile = leftTileTask.Result;
                ITile rightTile = rightTileTask.Result;

                // Handle collisions
                HandleCollisions(entity, bottomTile, topTile, leftTile, rightTile);

                // Prevent entity from moving outside the map bounds
                ClampEntityPosition(entity);
            });
        }

        private ITile GetTileFromCacheOrMap(Vector2 position)
        {
            if (!_tileCache.TryGetValue(position, out ITile tile))
            {
                tile = _map.GetTileAtPosition(position);
                _tileCache[position] = tile;
            }
            return tile;
        }

        private void HandleCollisions(IEntity entity, ITile bottomTile, ITile topTile, ITile leftTile, ITile rightTile)
        {
            if (bottomTile != null && bottomTile.IsCollidable)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
                entity.IsOnGround = true;
                int entityBottomY = entity.Bounds.Bottom;

                if (entityBottomY > bottomTile.Bounds.Top)
                {
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y - (entityBottomY - bottomTile.Bounds.Top));
                }
            }
            else
            {
                entity.IsOnGround = false;
            }

            if (topTile != null && topTile.IsCollidable)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
                int entityTopY = entity.Bounds.Top;
                if (entityTopY < topTile.Bounds.Bottom)
                {
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + (topTile.Bounds.Bottom - entityTopY));
                }
            }

            if (leftTile != null && leftTile.IsCollidable)
            {
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
                // move entity to the right by 1 pixel
                int entityLeftX = entity.Bounds.Left;
                if (entityLeftX < leftTile.Bounds.Right)
                {
                    entity.Position = new Vector2(entity.Position.X + (leftTile.Bounds.Right - entityLeftX), entity.Position.Y);
                }
            }

            if (rightTile != null && rightTile.IsCollidable)
            {
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
                // move entity to the left by 1 pixel
                int entityRightX = entity.Bounds.Right;
                if (entityRightX > rightTile.Bounds.Left)
                {
                    entity.Position = new Vector2(entity.Position.X - (entityRightX - rightTile.Bounds.Left), entity.Position.Y);
                }
            }
        }

        private void ClampEntityPosition(IEntity entity)
        {
            float entityWidth = entity.Bounds.Width;
            float entityHeight = entity.Bounds.Height;
            float mapWidthInPixels = _map.MapWidth * _map.TileWidth;
            float mapHeightInPixels = _map.MapHeight * _map.TileHeight;

            // Clamp the entity's position within the map bounds
            float clampedX = MathHelper.Clamp(entity.Position.X, 0, mapWidthInPixels - entityWidth);
            float clampedY = MathHelper.Clamp(entity.Position.Y, 0, mapHeightInPixels - entityHeight);

            entity.Position = new Vector2(clampedX, clampedY);
        }

        private bool IsEntityWithinBounds(IEntity entity)
        {
            float entityWidth = entity.Bounds.Width;
            float entityHeight = entity.Bounds.Height;
            float mapWidthInPixels = _map.MapWidth * _map.TileWidth;
            float mapHeightInPixels = _map.MapHeight * _map.TileHeight;

            return entity.Position.X >= 0 && entity.Position.X + entityWidth <= mapWidthInPixels
                && entity.Position.Y >= 0 && entity.Position.Y + entityHeight <= mapHeightInPixels;
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
                }
            }
        }
    }
}