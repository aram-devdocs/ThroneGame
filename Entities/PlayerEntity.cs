using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Controllers;
using ThroneGame.Utils;

namespace ThroneGame.Entities
{
    /// <summary>
    /// Represents the player entity in the game.
    /// </summary>
    public class PlayerEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity"/> class with the specified position and content manager.
        /// </summary>
        /// <param name="position">The initial position of the player.</param>
        /// <param name="content">The content manager used for loading textures.</param>
        public PlayerEntity(Vector2 position, ContentManager content, Game1 game, string name = "Player Entity") : base(name, position, game)
        {
            LoadAnimations(content);
            MovementController = new MovementController(position)
            {
                TakesInput = true
            };

        }

        /// <summary>
        /// Loads the animations for the player entity.
        /// </summary>
        /// <param name="content">The content manager used for loading textures.</param>
        private void LoadAnimations(ContentManager content)
        {
            AnimationController.AddAnimation("idle", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_idle"), 4, 0.13);
            AnimationController.AddAnimation("run", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_run"), 6, 0.1);
            AnimationController.AddAnimation("jump", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_jump"), 6, 0.1, false);
            AnimationController.AddAnimation("walk", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_walk"), 6, 0.13);
            AnimationController.AddAnimation("dead", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_death"), 6, 0.13, false);
            AnimationController.AddAnimation("crouch", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_crouch"), 1, 0.13);
            AnimationController.AddAnimation("hurt", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_hurt"), 3, 0.13);
            AnimationController.AddAnimation("attack1", content.Load<Texture2D>("PlayerSprites/Woodcutter/Woodcutter_attack1"), 6, 0.09, false);


            AnimationController.SetDefaultAnimationString("idle");

        }

        /// <summary>
        /// Updates the player entity's state based on its velocity and input.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public override void Update(GameTime gameTime)
        {
            UpdateAnimationState(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the animation state of the player entity based on its velocity and input.
        /// </summary>
        private void UpdateAnimationState(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();




            if (IsBeingAttacked)
            {
                AnimationController.SetState("hurt");
                return;
            }


            if (this.IsAttacking)
                return;

            // Handle combat
            // Right or left key is pressed
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left) && !this.IsAttacking)
            {
                AnimationController.SetState("attack1");
                this.IsAttacking = true;
                var attackDuration = 0.09; // TODO: Get this from the animation
                var frames = 6; // TODO: Get this from the animation
                this.AttackEndTime = gameTime.TotalGameTime.TotalSeconds + attackDuration * frames;
                this.IsFacingRight = keyboardState.IsKeyDown(Keys.Right) ? true : false;
                return;
            }


            // Handle movement
            if (Velocity.Y != 0)
            {
                AnimationController.SetState("jump");
            }
            else if (Velocity.X != 0)
            {
                AnimationController.SetState(keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) ? "run" : "walk");
            }
            else
            {
                AnimationController.SetState("idle");
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                AnimationController.SetState("crouch");
            }



        }
    }
}