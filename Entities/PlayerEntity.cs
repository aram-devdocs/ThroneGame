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
            AnimationController.AddAnimation("crouch", content.Load<Texture2D>("PlayerSprites/Shinobi/Crouch"), 1, 0.1);


            // this.Bounds should be reduced by 10% on horizontal axis, and 20% on vertical axis, using current Bounds values as reference
            HorizontalBoundsPadding = -80f;

        }


        public override void Update(GameTime gameTime)
        {

            // Update state based on velocity
            if (Velocity.Y < 0 || Velocity.Y > 0)
            {
                // Jumping state
                AnimationController.SetState("jump");
            }
            else if (Velocity.X < 0 || Velocity.X > 0)
            {
                // Walking or running state
                AnimationController.SetState((Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) ? "run" : "walk");
            }
            else
            {
                AnimationController.SetState("idle");
            }



            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Dead state
                AnimationController.SetState("crouch");
            }

            base.Update(gameTime);
        }
    }
}