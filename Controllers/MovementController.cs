using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the movement behavior of entities within the game.
    /// </summary>
    public class MovementController
    {
        private const float Speed = 100f;
        private const float SpeedUpRate = 9f;
        private const float SlowDownRate = 12f;
        private const float SprintAccelerationRate = 2f;
        private const float SprintMultiplier = 2f;
        private const float JumpStrength = 300f;
        private const float SlideBoost = 400f;
        private const float MinimumSlideBoostStartSpeed = 90f;
        private const float SlideBoostAccelerationRate = 40.2f;
        private const float CrouchDiveMaxSpeed = 200f;
        private const float CrouchDiveAccelerationRate = 20f;
        private bool isSlideBoostFinished;

        /// <summary>
        /// Handles the movement of the given entity based on keyboard input and game state.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="gameTime">The game time information.</param>
        public void HandleMovement(IEntity entity, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            bool sprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            float maxSpeed = sprinting ? Speed * SprintMultiplier : Speed;
            float accelerationRate = SpeedUpRate * (sprinting ? SprintAccelerationRate : 1f);

            try
            {
                if (state.IsKeyDown(Keys.S))
                {
                    HandleCrouch(entity, state);
                }
                else
                {
                    HandleHorizontalMovement(state, entity, maxSpeed, accelerationRate);
                }

                if (state.IsKeyDown(Keys.Space) && entity.IsOnGround && !state.IsKeyDown(Keys.S))
                {
                    HandleJump(entity);
                }
            }
            catch (Exception ex)
            {
                // Implement proper error handling (e.g., logging the error)
                Console.WriteLine($"Error in HandleMovement: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles crouching and related actions such as slide boosts and crouch dives.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="state">The current keyboard state.</param>
        private void HandleCrouch(IEntity entity, KeyboardState state)
        {
            if (!entity.IsOnGround)
            {
                HandleCrouchDive(entity);
            }

            bool isSprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            if (!isSlideBoostFinished && isSprinting && Math.Abs(entity.Velocity.X) > MinimumSlideBoostStartSpeed)
            {
                if (entity.IsFacingRight)
                {
                    BoostSlideRight(entity);
                }
                else
                {
                    BoostSlideLeft(entity);
                }
            }
            else
            {
                Decelerate(entity);
            }
        }

        /// <summary>
        /// Handles the crouch dive action.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        private void HandleCrouchDive(IEntity entity)
        {
            if (entity.Velocity.Y < CrouchDiveMaxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, Math.Min(CrouchDiveMaxSpeed, entity.Velocity.Y + CrouchDiveAccelerationRate));
            }
        }

        /// <summary>
        /// Handles horizontal movement based on the current keyboard state.
        /// </summary>
        /// <param name="state">The current keyboard state.</param>
        /// <param name="entity">The entity to control.</param>
        /// <param name="maxSpeed">The maximum speed the entity can achieve.</param>
        /// <param name="accelerationRate">The rate at which the entity accelerates.</param>
        private void HandleHorizontalMovement(KeyboardState state, IEntity entity, float maxSpeed, float accelerationRate)
        {
            if (state.IsKeyDown(Keys.A))
            {
                MoveLeft(entity, maxSpeed, accelerationRate);
            }
            else if (state.IsKeyDown(Keys.D))
            {
                MoveRight(entity, maxSpeed, accelerationRate);
            }
            else
            {
                Decelerate(entity);
            }
        }

        /// <summary>
        /// Moves the entity to the left.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="maxSpeed">The maximum speed the entity can achieve.</param>
        /// <param name="accelerationRate">The rate at which the entity accelerates.</param>
        private void MoveLeft(IEntity entity, float maxSpeed, float accelerationRate)
        {
            if (entity.Velocity.X >= -maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X - accelerationRate, entity.Velocity.Y);
            }
            entity.IsFacingRight = false;
            isSlideBoostFinished = false;
        }

        /// <summary>
        /// Moves the entity to the right.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="maxSpeed">The maximum speed the entity can achieve.</param>
        /// <param name="accelerationRate">The rate at which the entity accelerates.</param>
        private void MoveRight(IEntity entity, float maxSpeed, float accelerationRate)
        {
            if (entity.Velocity.X <= maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X + accelerationRate, entity.Velocity.Y);
            }
            entity.IsFacingRight = true;
            isSlideBoostFinished = false;
        }

        /// <summary>
        /// Decelerates the entity when there is no input for horizontal movement.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        private void Decelerate(IEntity entity)
        {
            if (entity.Velocity.X > 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Max(0, entity.Velocity.X - SlowDownRate), entity.Velocity.Y);
            }
            else if (entity.Velocity.X < 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Min(0, entity.Velocity.X + SlowDownRate), entity.Velocity.Y);
            }
            else if (!entity.IsOnGround)
            {
                isSlideBoostFinished = false;
            }
        }

        /// <summary>
        /// Boosts the entity's slide to the right.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        private void BoostSlideRight(IEntity entity)
        {
            if (entity.Velocity.X < SlideBoost)
            {
                entity.Velocity = new Vector2(Math.Min(SlideBoost, entity.Velocity.X + SlideBoostAccelerationRate), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }

        /// <summary>
        /// Boosts the entity's slide to the left.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        private void BoostSlideLeft(IEntity entity)
        {
            if (entity.Velocity.X > -SlideBoost)
            {
                entity.Velocity = new Vector2(Math.Max(-SlideBoost, entity.Velocity.X - SlideBoostAccelerationRate), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }

        /// <summary>
        /// Handles the jump action for the entity.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        private void HandleJump(IEntity entity)
        {
            entity.Velocity = new Vector2(entity.Velocity.X, -JumpStrength);
            entity.IsOnGround = false;
        }
    }
}