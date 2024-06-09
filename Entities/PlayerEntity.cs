using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount)
            : base(texture, position, frameWidth, frameHeight, frameCount)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Update state based on velocity
            if (Velocity.Y < 0 || Velocity.Y > 0)
            {
                State = "jump";
            }
            else if (Velocity.X < 0 || Velocity.X > 0)
            {
                State = "run";
            }
            else
            {
                State = "idle";
            }

            base.Update(gameTime);
        }
    }
}