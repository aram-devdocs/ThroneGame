using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Controllers;

namespace ThroneGame.Entities
{
    /// <summary>
    /// Represents the player entity in the game.
    /// </summary>
    public class EnemyEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyEntity"/> class with the specified position and content manager.
        /// </summary>
        /// <param name="position">The initial position of the player.</param>
        /// <param name="content">The content manager used for loading textures.</param>
        public EnemyEntity(Vector2 position, ContentManager content, Game1 game, string name = "Enemy Entity")
            : base(name, position, game)
        {
            LoadAnimations(content);
            MovementController = new MovementController();
        }

        /// <summary>
        /// Loads the animations for the player entity.
        /// </summary>
        /// <param name="content">The content manager used for loading textures.</param>
        private void LoadAnimations(ContentManager content)
        {
            AnimationController.AddAnimation("idle", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_idle"), 4, 0.13);
            AnimationController.AddAnimation("run", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_run"), 6, 0.1);
            AnimationController.AddAnimation("jump", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_jump"), 6, 0.1, false);
            AnimationController.AddAnimation("walk", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_walk"), 6, 0.13);
            AnimationController.AddAnimation("dead", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_death"), 6, 0.13, false);
            // AnimationController.AddAnimation("crouch", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_crouch"), 1, 0.13);
            AnimationController.AddAnimation("hurt", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_hurt"), 3, 0.13);
            AnimationController.AddAnimation("attack1", content.Load<Texture2D>("PlayerSprites/GraveRobber/GraveRobber_attack1"), 6, 0.09, false);

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

            if (IsBeingAttacked)
            {
                AnimationController.SetState("hurt");
                return;
            }

            // Handle movement based on velocity
            if (Velocity.Y != 0)
            {
                AnimationController.SetState("jump");
            }
            else if (Velocity.X != 0)
            {
                AnimationController.SetState("walk");
            }
            else
            {
                AnimationController.SetState("idle");
            }

        }
    }
}