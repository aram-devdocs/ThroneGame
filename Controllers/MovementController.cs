using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;
using ThroneGame.Utils;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the movement behavior of entities within the game.
    /// </summary>
    public class MovementController
    {
        public float Speed { get; set; } = 175;
        public float SpeedUpRate { get; set; } = 120f;
        public float SlowDownRate { get; set; } = 550f;

        public float PivotSpeed { get; set; } = 400f;
        public float SprintAccelerationRate { get; set; } = 1.8f;
        public float SprintMultiplier { get; set; } = 2.14f;
        public float JumpStrength { get; set; } = 220f;
        public float SlideBoostMaxSpeed { get; set; } = 500f;
        public float MinimumSlideBoostStartSpeed { get; set; } = 80f;
        public float SlideBoostAccelerationRate { get; set; } = 800.2f;
        public float CrouchDiveMaxSpeed { get; set; } = 200f;
        public float CrouchDiveAccelerationRate { get; set; } = 20f;

        private bool isSlideBoostFinished;
        private Queue<Vector2> pathPoints;
        private Vector2? targetPosition;
        private Vector2? startPosition;
        public bool TakesInput = false;

        /// <summary>
        /// Gets or sets the target position for pathfinding.
        /// </summary>
        public Vector2? TargetPosition
        {
            get => targetPosition;
            set
            {
                targetPosition = value;
                if (targetPosition.HasValue)
                {
                    CalculatePathToTarget();
                }
            }
        }

        public MovementController(Vector2? startPosition = null)
        {
            this.startPosition = startPosition;
            pathPoints = new Queue<Vector2>();
        }

        /// <summary>
        /// Handles the movement of the given entity based on keyboard input and game state.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="gameTime">The game time information.</param>
        public void HandleMovement(IEntity entity, GameTime gameTime)
        {





            if (!entity.IsBeingAttacked)
            {
                if (TargetPosition.HasValue && pathPoints.Count > 0)
                {
                    FollowPath(entity, gameTime);
                }
                else if (TakesInput)
                {
                    HandleUserInput(entity, gameTime);
                }
            }



            var state = Keyboard.GetState();
            float deltaTime = GameUtils.GetDeltaTime(gameTime);
            // If no movement keys are pressed, decelerate the entity. If the entity is not taking input, decelerate it. TODO, 
            if (entity.IsOnGround && entity.Velocity.X != 0 && (TakesInput && !state.IsKeyDown(Keys.A) && !state.IsKeyDown(Keys.D)) || !TakesInput)
            {
                Decelerate(entity, deltaTime);
            }
        }

        /// <summary>
        /// Handles the movement of the entity based on the user input.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="gameTime">The game time information.</param>
        private void HandleUserInput(IEntity entity, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            bool sprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            float maxSpeed = sprinting ? Speed * SprintMultiplier : Speed;
            float accelerationRate = SpeedUpRate * (sprinting ? SprintAccelerationRate : 1f);
            float deltaTime = GameUtils.GetDeltaTime(gameTime);

            try
            {
                if (state.IsKeyDown(Keys.S))
                {
                    HandleCrouch(entity, state, deltaTime);
                }
                else
                {
                    HandleHorizontalMovement(state, entity, maxSpeed, accelerationRate, deltaTime);
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
        /// Calculates the path to the target position and queues up the points to navigate to.
        /// </summary>
        private void CalculatePathToTarget()
        {
            // Placeholder for the pathfinding algorithm
            pathPoints = new Queue<Vector2>();
            if (TargetPosition.HasValue)
            {
                // Simplified pathfinding: direct line (you may replace this with a proper pathfinding algorithm)
                Vector2 startPosition = this.startPosition ?? Vector2.Zero;
                Vector2 endPosition = TargetPosition.Value;

                Vector2 direction = Vector2.Normalize(endPosition - startPosition);
                float distance = Vector2.Distance(startPosition, endPosition);
                int steps = (int)Math.Ceiling(distance / Speed);

                for (int i = 1; i <= steps; i++)
                {
                    pathPoints.Enqueue(startPosition + direction * Speed * i);
                }

                pathPoints.Enqueue(endPosition);
            }
        }

        /// <summary>
        /// Follows the calculated path towards the target position.
        /// </summary>
        /// <param name="entity">The entity to control.</param>
        /// <param name="gameTime">The game time information.</param>
        private void FollowPath(IEntity entity, GameTime gameTime)
        {
            if (pathPoints.Count == 0)
            {
                TargetPosition = null;
                return;
            }

            float deltaTime = GameUtils.GetDeltaTime(gameTime);
            Vector2 currentTarget = pathPoints.Peek();
            Vector2 direction = Vector2.Normalize(currentTarget - entity.Position);
            entity.Velocity = direction * (Speed * deltaTime);

            // Check if entity has reached the current target point
            if (Vector2.Distance(entity.Position, currentTarget) < Speed * deltaTime)
            {
                entity.Position = currentTarget;
                pathPoints.Dequeue();
            }
        }

        public void SetTargetPosition(Vector2 target)
        {
            TargetPosition = target;
        }

        // Existing methods for crouching, jumping, etc.
        private void HandleCrouch(IEntity entity, KeyboardState state, float deltaTime)
        {
            if (!entity.IsOnGround)
            {
                HandleCrouchDive(entity, deltaTime);
            }

            bool isSprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            if (!isSlideBoostFinished && isSprinting && Math.Abs(entity.Velocity.X) > MinimumSlideBoostStartSpeed)
            {
                if (entity.IsFacingRight)
                {
                    BoostSlideRight(entity, deltaTime);
                }
                else
                {
                    BoostSlideLeft(entity, deltaTime);
                }
            }
            else
            {
                Decelerate(entity, deltaTime);
            }
        }

        private void HandleCrouchDive(IEntity entity, float deltaTime)
        {
            if (entity.Velocity.Y < CrouchDiveMaxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, Math.Min(CrouchDiveMaxSpeed, entity.Velocity.Y + CrouchDiveAccelerationRate * deltaTime));
            }
        }

        private void HandleHorizontalMovement(KeyboardState state, IEntity entity, float maxSpeed, float accelerationRate, float deltaTime)
        {
            bool isMoveLeftPressed = state.IsKeyDown(Keys.A);
            bool isMoveRightPressed = state.IsKeyDown(Keys.D);

            //   Handle if both A and D are pressed, use the most recently pressed key
            if (isMoveLeftPressed && isMoveRightPressed)
            {
                if (entity.IsFacingRight)
                {
                    MoveRight(entity, maxSpeed, accelerationRate, deltaTime);
                }
                else
                {
                    MoveLeft(entity, maxSpeed, accelerationRate, deltaTime);
                }
            }
            else if (isMoveLeftPressed)
            {
                MoveLeft(entity, maxSpeed, accelerationRate, deltaTime);
            }
            else if (isMoveRightPressed)
            {
                MoveRight(entity, maxSpeed, accelerationRate, deltaTime);
            }
            else
            {
                Decelerate(entity, deltaTime);
            }
        }
        private void MoveLeft(IEntity entity, float maxSpeed, float accelerationRate, float deltaTime)

        {
            if (entity.Velocity.X > 0)
            {
                // If going right, apply pivot speed to turn around
                entity.Velocity = new Vector2(entity.Velocity.X - (PivotSpeed * deltaTime), entity.Velocity.Y);
            }

            else if (entity.Velocity.X >= -maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X - (accelerationRate * deltaTime), entity.Velocity.Y);
            }

            if (!entity.IsAttacking) entity.IsFacingRight = false;
            isSlideBoostFinished = false;
        }

        private void MoveRight(IEntity entity, float maxSpeed, float accelerationRate, float deltaTime)
        {

            if (entity.Velocity.X < 0)
            {
                // If going left, apply pivot speed to turn aroun
                entity.Velocity = new Vector2(entity.Velocity.X + (PivotSpeed * deltaTime), entity.Velocity.Y);
            }
            else if (entity.Velocity.X <= maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X + (accelerationRate * deltaTime), entity.Velocity.Y);
            }
            if (!entity.IsAttacking) entity.IsFacingRight = true;
            isSlideBoostFinished = false;
        }

        private void Decelerate(IEntity entity, float deltaTime)
        {
            if (entity.Velocity.X > 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Max(0, entity.Velocity.X - (SlowDownRate * deltaTime)), entity.Velocity.Y);
            }
            else if (entity.Velocity.X < 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Min(0, entity.Velocity.X + (SlowDownRate * deltaTime)), entity.Velocity.Y);
            }
            else if (!entity.IsOnGround)
            {
                isSlideBoostFinished = false;
            }
        }

        private void BoostSlideRight(IEntity entity, float deltaTime)
        {
            if (entity.Velocity.X < SlideBoostMaxSpeed)
            {
                entity.Velocity = new Vector2(Math.Min(SlideBoostMaxSpeed, entity.Velocity.X + (SlideBoostAccelerationRate * deltaTime)), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }

        private void BoostSlideLeft(IEntity entity, float deltaTime)
        {
            if (entity.Velocity.X > -SlideBoostMaxSpeed)
            {
                entity.Velocity = new Vector2(Math.Max(-SlideBoostMaxSpeed, entity.Velocity.X - (SlideBoostAccelerationRate * deltaTime)), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }
        private void HandleJump(IEntity entity)
        {
            entity.Velocity = new Vector2(entity.Velocity.X, -JumpStrength);
            entity.IsOnGround = false;
        }
    }
}