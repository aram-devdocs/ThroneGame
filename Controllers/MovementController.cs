using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;

namespace ThroneGame.Controllers
{
    public class MovementController
    {
        private const float speed = 100f;
        private const float speedUpRate = 9f;
        private const float slowDownRate = 12f;
        private const float sprintAccelerationRate = 1.2f;
        private const float sprintMultiplier = 2f;
        private const float jumpStrength = 200f;
        private const float slideBoost = 400f;
        private const float slideBoostAccelerationRate = 40.2f;
        private bool isSlideBoostFinished;

        public void HandleMovement(IEntity entity, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            bool sprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            float maxSpeed = sprinting ? speed * sprintMultiplier : speed;
            float accelerationRate = speedUpRate * (sprinting ? sprintAccelerationRate : 1f);

            try
            {
                if (state.IsKeyDown(Keys.S) && entity.IsOnGround)
                {
                    HandleCrouch(entity);
                }
                else
                {
                    HandleHorizontalMovement(state, entity, maxSpeed, accelerationRate);
                }

                if (state.IsKeyDown(Keys.Space) && entity.IsOnGround)
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

        private void HandleCrouch(IEntity entity)
        {
            if (!isSlideBoostFinished)
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

        private void HandleHorizontalMovement(KeyboardState state, IEntity entity, float maxSpeed, float accelerationRate)
        {
            switch (state.GetPressedKeys())
            {
                case var keys when keys.Contains(Keys.A):
                    MoveLeft(entity, maxSpeed, accelerationRate);
                    break;
                case var keys when keys.Contains(Keys.D):
                    MoveRight(entity, maxSpeed, accelerationRate);
                    break;
                default:
                    Decelerate(entity);
                    break;
            }
        }

        private void MoveLeft(IEntity entity, float maxSpeed, float accelerationRate)
        {
            if (entity.Velocity.X >= -maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X - accelerationRate, entity.Velocity.Y);
            }
            entity.IsFacingRight = false;
            isSlideBoostFinished = false;
        }

        private void MoveRight(IEntity entity, float maxSpeed, float accelerationRate)
        {
            if (entity.Velocity.X <= maxSpeed)
            {
                entity.Velocity = new Vector2(entity.Velocity.X + accelerationRate, entity.Velocity.Y);
            }
            entity.IsFacingRight = true;
            isSlideBoostFinished = false;
        }

        private void Decelerate(IEntity entity)
        {
            if (entity.Velocity.X > 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Max(0, entity.Velocity.X - slowDownRate), entity.Velocity.Y);
            }
            else if (entity.Velocity.X < 0 && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(Math.Min(0, entity.Velocity.X + slowDownRate), entity.Velocity.Y);
            }
            else if (!entity.IsOnGround)
            {
                isSlideBoostFinished = false;
            }
        }

        private void BoostSlideRight(IEntity entity)
        {
            if (entity.Velocity.X < slideBoost)
            {
                entity.Velocity = new Vector2(Math.Min(slideBoost, entity.Velocity.X + slideBoostAccelerationRate), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }

        private void BoostSlideLeft(IEntity entity)
        {
            if (entity.Velocity.X > -slideBoost)
            {
                entity.Velocity = new Vector2(Math.Max(-slideBoost, entity.Velocity.X - slideBoostAccelerationRate), entity.Velocity.Y);
            }
            else
            {
                isSlideBoostFinished = true;
            }
        }

        private void HandleJump(IEntity entity)
        {
            entity.Velocity = new Vector2(entity.Velocity.X, -jumpStrength);
        }
    }
}