using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ThroneGame.Entities
{
    public class PlayerEntity : Entity
    {

        public PlayerEntity(Vector2 position, ContentManager content)
            : base(position)
        {
            // Load textures and add animations
            AnimationController.AddAnimation("idle", content.Load<Texture2D>("PlayerSprites/Shinobi/Idle"), 6, 0.1);
            AnimationController.AddAnimation("run", content.Load<Texture2D>("PlayerSprites/Shinobi/Run"), 8, 0.1);
            AnimationController.AddAnimation("jump", content.Load<Texture2D>("PlayerSprites/Shinobi/Jump"), 12, 0.1, false);
            AnimationController.AddAnimation("walk", content.Load<Texture2D>("PlayerSprites/Shinobi/Walk"), 8, 0.1);
            AnimationController.AddAnimation("dead", content.Load<Texture2D>("PlayerSprites/Shinobi/Dead"), 4, 0.27, false);
        }


        public override void Update(GameTime gameTime)
        {

            // Update state based on velocity
            if (Velocity.Y < 0 || Velocity.Y > 0)
            {
                AnimationController.SetState("jump");
            }
            else if (Velocity.X < 0 || Velocity.X > 0)
            {
                AnimationController.SetState((Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) ? "run" : "walk");
            }
            else
            {
                AnimationController.SetState("idle");
            }

            // if Keydown is S, set state to dead
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                AnimationController.SetState("dead");
            }

            base.Update(gameTime);
        }
    }
}