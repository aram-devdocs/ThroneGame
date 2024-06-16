using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;
using ThroneGame.Scenes;
using ThroneGame.Utils;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the combat actions of an entity.
    /// </summary>
    public class CombatController
    {
        private readonly IEntity _entity;
        private Game1 _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatController"/> class.
        /// </summary>
        /// <param name="entity">The entity that this controller is assigned to.</param>

        public CombatController(IEntity entity, Game1 game)
        {
            _entity = entity;
            _game = game;
        }




        /// <summary>
        /// Has the entity attacked this cycle?
        /// </summary>
        private bool attackedThisCycle = false;




        /// <summary>
        /// Updates the combat state of the entity based on input.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {

            bool IsAttacking = _entity.IsAttacking;
            double AttackEndTime = _entity.AttackEndTime;

            // IF enemy is attacing and hasnt attacked this cycle, attack
            if (IsAttacking && !attackedThisCycle)
            {
                // Attack
                attackedThisCycle = true;

                System.Console.WriteLine("Attacking");
                var entitiesInRange = GetEntitiesInRange();
                // System.Console.WriteLine( return name of each enemy in entitiesInRange)
                System.Console.WriteLine("Entities in range:" + entitiesInRange.Count());
                foreach (var entity in entitiesInRange)
                {
                    System.Console.WriteLine(entity.Name);
                }

            }

            // if attackedThisCycle and the attack has ended, reset attackedThisCycle
            if (attackedThisCycle && gameTime.TotalGameTime.TotalSeconds > AttackEndTime)
            {
                attackedThisCycle = false;
            }



        }


        /// <summary>
        /// Draw the debug information for the combat controller.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Get the bounds of the entity's attack
            Rectangle attackBounds = GetAttackBounds();

            // Draw the bounds of the entity's attack
            TextureUtils.DebugBorder(spriteBatch, attackBounds.X, attackBounds.Y, attackBounds.Width, attackBounds.Height);

        }

        /// <summary>
        /// Gets the entities that are in range of the entity's attack.
        /// </summary>
        /// <returns name="Enumerable<IEntity>">The entities that are in range of the entity's attack.</returns>
        public IEnumerable<IEntity> GetEntitiesInRange()
        {
            // Get the entities in the scene
            var entities = _game.currentScene.Entities;

            // Create a list to store the entities in range
            var entitiesInRange = new List<IEntity>();


            // Loop through each entity in the scene
            foreach (var entity in entities)
            {
                // If the entity is the same as the entity that this controller is assigned to, skip it
                if (entity == _entity)
                {
                    continue;
                }

                // If the entity is in range of the entity's attack, add it to the list
                if (IsEntityInRange(entity))
                {
                    entitiesInRange.Add(entity);
                }
            }

            // Return the list of entities in range
            return entitiesInRange;
        }

        /// <summary>
        /// Determines if the specified entity is in range of the entity's attack.
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns name="bool">True if the entity is in range of the entity's attack; otherwise, false.</returns>
        private bool IsEntityInRange(IEntity entity)
        {
            // Get the bounds of the entity
            var entityBounds = entity.Bounds;

            // Get the bounds of the entity's attack
            var attackBounds = GetAttackBounds();

            // Check if the entity's bounds intersects with the attack bounds
            return entityBounds.Intersects(attackBounds);
        }

        /// <summary>
        /// Gets the bounds of the entity's attack.
        /// </summary>
        /// <returns name="Rectangle">The bounds of the entity's attack.</returns>
        private Rectangle GetAttackBounds()
        {
            // Get the bounds of the entity
            var entityBounds = _entity.Bounds;

            // Calculate the attack position based on the entity's facing direction
            var attackPosition = _entity.IsFacingRight
                ? new Vector2(entityBounds.Right, entityBounds.Center.Y - 15)
                : new Vector2(entityBounds.Left - 30, entityBounds.Center.Y - 15);

            // Create and return the bounds of the entity's attack
            return new Rectangle((int)attackPosition.X, (int)attackPosition.Y, 30, 30);
        }




    }
}