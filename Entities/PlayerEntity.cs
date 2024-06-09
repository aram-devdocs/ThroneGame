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
            AddAnimation("idle", content.Load<Texture2D>("PlayerSprites/Shinobi/Idle"), 6, 0.1);
            AddAnimation("run", content.Load<Texture2D>("PlayerSprites/Shinobi/Run"), 8, 0.1);
            AddAnimation("jump", content.Load<Texture2D>("PlayerSprites/Shinobi/Jump"), 12, 0.1, false);
            AddAnimation("walk", content.Load<Texture2D>("PlayerSprites/Shinobi/Walk"), 8, 0.1);
            AddAnimation("dead", content.Load<Texture2D>("PlayerSprites/Shinobi/Dead"), 4, 0.27, false);
        }


        public override void Update(GameTime gameTime)
        {

            // Update state based on velocity
            if (Velocity.Y < 0 || Velocity.Y > 0)
            {
                SetState("jump");
            }
            else if (Velocity.X < 0 || Velocity.X > 0)
            {

                SetState((Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) ? "run" : "walk");


            }
            else
            {
                SetState("idle");
            }

            // if Keydown is S, set state to dead
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                SetState("dead");
            }

            base.Update(gameTime);
        }
    }
}