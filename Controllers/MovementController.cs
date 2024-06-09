using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;

namespace ThroneGame.Controllers
{
    public class MovementController
    {
        private const float Speed = 100f;
        private const float JumpStrength = 300f;

        public void HandleMovement(IEntity entity, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                entity.Velocity = new Vector2(-Speed, entity.Velocity.Y);
            }
            else if (state.IsKeyDown(Keys.D))
            {
                entity.Velocity = new Vector2(Speed, entity.Velocity.Y);
            }
            else
            {
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
            }

            if (state.IsKeyDown(Keys.Space) && entity.IsOnGround)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, -JumpStrength);
            }
        }
    }
}