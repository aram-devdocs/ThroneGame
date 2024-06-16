using System;
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
        private float AttackBufferTime = 0.2f; // We want to attack right before the animation ends, so we buffer the attack time by 0.5 seconds
        private float knockbackSpeed = 250f;
        private float upwardsKnockbackSpeed = 100f;

        private double stunDuration = 1f;

        private float damage = 10f;


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


            // If this entity is being attacked, return
            if (_entity.IsBeingAttacked)
            {
                return;
            }


            bool IsAttacking = _entity.IsAttacking;
            double AttackEndTime = _entity.AttackEndTime;
            // IF enemy is attacing and hasnt attacked this cycle, attack
            if (IsAttacking && !attackedThisCycle && gameTime.TotalGameTime.TotalSeconds > AttackEndTime - AttackBufferTime)
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
                    entity.CombatController.IsAttacked(_entity, gameTime, damage);
                }


            }

            // if attackedThisCycle and the attack has ended, reset attackedThisCycle
            if (attackedThisCycle && gameTime.TotalGameTime.TotalSeconds > AttackEndTime)
            {
                attackedThisCycle = false;
                _entity.IsAttacking = false;

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


        public void IsAttacked(IEntity attacker, GameTime gameTime, float damage)
        {
            // System.Console.WriteLine("Attacked");
            // using the attackers position, knock back this entity

            Vector2 attackersPosition = attacker.Position;

            // set the velocity of the entity to be knocked back. we will slow down the velocity over time in the physics controller



            //    normalize the vector between the attacker and the entity
            Vector2 knockbackDirection = Vector2.Normalize(_entity.Position - attackersPosition);
            knockbackDirection.Y = -1;
            _entity.Velocity = knockbackDirection * knockbackSpeed;
            // _entity.Velocity.Y = -upwardsKnockbackSpeed; // set the _entity.Velocity instead as a new vector2 with -upwardsKnockbackSpeed as the y value
            _entity.Velocity = new Vector2(_entity.Velocity.X, -upwardsKnockbackSpeed);

            _entity.IsOnGround = false;
            _entity.IsBeingAttacked = true;
            _entity.StunEndTime = gameTime.TotalGameTime.TotalSeconds + stunDuration;
            _entity.Health -= (int)Math.Ceiling(damage);

        }




    }
}