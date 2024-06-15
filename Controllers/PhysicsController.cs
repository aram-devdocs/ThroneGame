using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public PhysicsController()
        {
            _entities = new List<IEntity>();

        }



        public void LoadMap(IMap map)
        {
            _map = map;
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


                if (!entity.IsOnGround)
                {
                    // Apply gravity

                    entity.Velocity = new Vector2(entity.Velocity.X, entity.Velocity.Y + Gravity * deltaTime);
                }




                Vector2 bottomCenter = new Vector2(entity.Bounds.X + entity.Bounds.Width / 2, entity.Bounds.Bottom);
                Vector2 topCenter = new Vector2(entity.Bounds.X + entity.Bounds.Width / 2, entity.Bounds.Top);
                Vector2 leftCenter = new Vector2(entity.Bounds.Left, entity.Bounds.Y + entity.Bounds.Height / 2);
                Vector2 rightCenter = new Vector2(entity.Bounds.Right, entity.Bounds.Y + entity.Bounds.Height / 2);

                ITile BottomTile = _map.GetTileAtPosition(bottomCenter);
                ITile TopTile = _map.GetTileAtPosition(topCenter);
                ITile LeftTile = _map.GetTileAtPosition(leftCenter);
                ITile RightTile = _map.GetTileAtPosition(rightCenter);


                if (BottomTile != null && BottomTile.IsCollidable)
                {
                    System.Console.WriteLine("BottomTile");
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }
                else
                {
                    entity.IsOnGround = false;
                }

                if (TopTile != null && TopTile.IsCollidable)
                {
                    System.Console.WriteLine("TopTile");
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }

                if (LeftTile != null && LeftTile.IsCollidable)
                {
                    System.Console.WriteLine("LeftTile");
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                    // move entity to the right by 1 pixel
                    entity.Position = new Vector2(entity.Position.X + 1, entity.Position.Y);
                }

                if (RightTile != null && RightTile.IsCollidable)
                {
                    System.Console.WriteLine("RightTile");
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                    // move entity to the left by 1 pixel
                    entity.Position = new Vector2(entity.Position.X - 1, entity.Position.Y);
                }

                // Additional collision handling logic for TopTile, LeftTile, RightTile can be added here
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

                }
            }
        }


    }
}