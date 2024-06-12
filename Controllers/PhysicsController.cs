using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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


        private void HandleCollision(IEntity entity, ref Vector2 newPosition, ITile tile)
        {
            if (entity.Vertices != null && entity.Vertices.Length > 0)
            {
                // Collision detection using vertices
                if (IsPolygonColliding(entity.Vertices, tile.Bounds))
                {
                    // Handle collision response (adjust newPosition and stop velocity)
                    StopVelocityOnCollision(entity, ref newPosition, tile.Bounds);
                }
            }
            else
            {
                // Collision detection using bounds
                Rectangle entityBounds = entity.Bounds;
                Rectangle tileBounds = tile.Bounds;

                if (IsColliding(entityBounds, tileBounds))
                {
                    StopVelocityOnCollision(entity, ref newPosition, tileBounds);
                }
            }
        }

        /// <summary>
        /// Checks if a polygon is colliding with a rectangle.
        /// </summary>
        /// <param name="polygonVertices">The vertices of the polygon.</param>
        /// <param name="rectangle">The rectangle to check collision against.</param>
        /// <returns>True if there is a collision, false otherwise.</returns>
        private bool IsPolygonColliding(Vector2[] polygonVertices, Rectangle rectangle)
        {
            List<Vector2> rectVertices = new List<Vector2>
    {
        new Vector2(rectangle.Left, rectangle.Top),
        new Vector2(rectangle.Right, rectangle.Top),
        new Vector2(rectangle.Right, rectangle.Bottom),
        new Vector2(rectangle.Left, rectangle.Bottom)
    };

            return IsPolygonIntersecting(polygonVertices, rectVertices.ToArray());
        }

        /// <summary>
        /// Checks if two polygons are intersecting.
        /// </summary>
        /// <param name="polygonA">The vertices of the first polygon.</param>
        /// <param name="polygonB">The vertices of the second polygon.</param>
        /// <returns>True if the polygons are intersecting, false otherwise.</returns>
        private bool IsPolygonIntersecting(Vector2[] polygonA, Vector2[] polygonB)
        {
            // Check for separating axis for both polygons
            return !IsSeparatingAxis(polygonA, polygonB) && !IsSeparatingAxis(polygonB, polygonA);
        }

        /// <summary>
        /// Checks if there is a separating axis between two polygons.
        /// </summary>
        /// <param name="polygonA">The vertices of the first polygon.</param>
        /// <param name="polygonB">The vertices of the second polygon.</param>
        /// <returns>True if there is a separating axis, false otherwise.</returns>
        private bool IsSeparatingAxis(Vector2[] polygonA, Vector2[] polygonB)
        {
            for (int i = 0; i < polygonA.Length; i++)
            {
                Vector2 edge = polygonA[(i + 1) % polygonA.Length] - polygonA[i];
                Vector2 axis = new Vector2(-edge.Y, edge.X);

                float minA = float.MaxValue, maxA = float.MinValue;
                foreach (var vertex in polygonA)
                {
                    float projection = Vector2.Dot(vertex, axis);
                    minA = Math.Min(minA, projection);
                    maxA = Math.Max(maxA, projection);
                }

                float minB = float.MaxValue, maxB = float.MinValue;
                foreach (var vertex in polygonB)
                {
                    float projection = Vector2.Dot(vertex, axis);
                    minB = Math.Min(minB, projection);
                    maxB = Math.Max(maxB, projection);
                }

                if (maxA < minB || maxB < minA)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Stops the velocity of an entity based on collision with a rectangle.
        /// </summary>
        /// <param name="entity">The entity to adjust.</param>
        /// <param name="newPosition">The new position of the entity.</param>
        /// <param name="rectangle">The rectangle that the entity collided with.</param>
        private void StopVelocityOnCollision(IEntity entity, ref Vector2 newPosition, Rectangle rectangle)
        {
            Rectangle entityBounds = entity.Bounds;
            Rectangle tileBounds = rectangle;

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
                        // newPosition.Y = tileBounds.Bottom;
                    }
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
            }
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

                    // Debug Vertices of entity
                    if (entity.Vertices != null && entity.Vertices.Length > 0)
                    {
                        for (int i = 0; i < entity.Vertices.Length; i++)
                        {
                            Vector2 vertexA = entity.Vertices[i];
                            Vector2 vertexB = entity.Vertices[(i + 1) % entity.Vertices.Length];
                            TextureUtils.DebugLine(spriteBatch, vertexA, vertexB);
                        }
                    }


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

            bool isFloating = false;

            // Handle collisions with tiles
            Parallel.ForEach(nearbyTiles, tile =>
            {
                if (tile.IsCollidable)
                {
                    HandleCollision(entity, ref newPosition, tile);

                }

                // if the entity has tiles below its y but not within the width of its x, it is not on the ground as long as there are no tiles below its y within the width of its x, and so it is floating
                if (entity.Bounds.Bottom == tile.Bounds.Top && entity.Bounds.Right < tile.Bounds.Left || entity.Bounds.Left > tile.Bounds.Right)
                {
                    isFloating = true;
                }


            });

            if (!isFloating)
            {
                entity.IsOnGround = false;
            }

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
        // /// <param name="tile">The tile that the entity collided with.</param>
        // private void HandleCollision(IEntity entity, ref Vector2 newPosition, ITile tile)
        // {
        //     Rectangle entityBounds = entity.Bounds;
        //     Rectangle tileBounds = tile.Bounds;

        //     if (IsColliding(entityBounds, tileBounds))
        //     {
        //         Rectangle intersection = Rectangle.Intersect(entityBounds, tileBounds);

        //         if (intersection.Width < intersection.Height)
        //         {
        //             // Collision on left or right
        //             if (entityBounds.Center.X < tileBounds.Center.X)
        //             {
        //                 // Collision on the left
        //                 newPosition.X = tileBounds.Left - entityBounds.Width - 1; // Add buffer of 1 pixel
        //             }
        //             else
        //             {
        //                 // Collision on the right
        //                 newPosition.X = tileBounds.Right + 1; // Add buffer of 1 pixel
        //             }
        //             entity.Velocity = new Vector2(0, entity.Velocity.Y);
        //         }
        //         else
        //         {
        //             // Collision on top or bottom
        //             if (entityBounds.Center.Y < tileBounds.Center.Y)
        //             {
        //                 // Collision on top
        //                 newPosition.Y = tileBounds.Top - entityBounds.Height;
        //                 entity.IsOnGround = true;
        //             }
        //             else
        //             {
        //                 // Collision on the bottom
        //                 newPosition.Y = tileBounds.Bottom;
        //             }
        //             entity.Velocity = new Vector2(entity.Velocity.X, 0);
        //         }
        //     }
        // }

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