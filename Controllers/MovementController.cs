using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;

namespace ThroneGame.Controllers
{
    public class MovementController
    {
        private const float Speed = 100f;

        private const float SpeedUpRate = 9f;
        private const float SlowDownRate = 12f;

        private const float SprintAccelerationRate = 1.2f;
        private const float SprintMultiplier = 2f;
        private const float JumpStrength = 200f;

        public void HandleMovement(IEntity entity, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            bool sprinting = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);

            float maxSpeed = sprinting ? Speed * SprintMultiplier : Speed;
            float accelerationRate = SpeedUpRate * (sprinting ? SprintAccelerationRate : 1f);

            if (state.IsKeyDown(Keys.A))
            {
                // speed up gradually
                if (entity.Velocity.X >= -maxSpeed) entity.Velocity = new Vector2(entity.Velocity.X - accelerationRate, entity.Velocity.Y);
                entity.IsFacingRight = false;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                // entity.Velocity = new Vector2(state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift) ? Speed * SprintMultiplier : Speed, entity.Velocity.Y);
                if (entity.Velocity.X <= maxSpeed) entity.Velocity = new Vector2(entity.Velocity.X + accelerationRate, entity.Velocity.Y);
                entity.IsFacingRight = true;
            }
            else
            {
                if (entity.Velocity.X > 0 && entity.IsOnGround)
                {
                    entity.Velocity = new Vector2(Math.Max(0, entity.Velocity.X - SlowDownRate), entity.Velocity.Y);
                }
                else if (entity.Velocity.X < 0 && entity.IsOnGround)
                {
                    entity.Velocity = new Vector2(Math.Min(0, entity.Velocity.X + SlowDownRate), entity.Velocity.Y);
                }
            }

            if (state.IsKeyDown(Keys.Space) && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, -JumpStrength);
            }
        }
    }
}