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
        }

        /// <summary>
        /// Loads the animations for the player entity.
        /// </summary>
        /// <param name="content">The content manager used for loading textures.</param>
        private void LoadAnimations(ContentManager content)
        {
            AnimationController.AddAnimation("idle", content.Load<Texture2D>("PlayerSprites/Shinobi/Idle"), 6, 0.1);
            AnimationController.AddAnimation("run", content.Load<Texture2D>("PlayerSprites/Shinobi/Run"), 8, 0.1);
            AnimationController.AddAnimation("jump", content.Load<Texture2D>("PlayerSprites/Shinobi/Jump"), 12, 0.1, false);
            AnimationController.AddAnimation("walk", content.Load<Texture2D>("PlayerSprites/Shinobi/Walk"), 8, 0.1);
            AnimationController.AddAnimation("dead", content.Load<Texture2D>("PlayerSprites/Shinobi/Dead"), 4, 0.27, false);
            AnimationController.AddAnimation("crouch", content.Load<Texture2D>("PlayerSprites/Shinobi/Crouch"), 1, 0.1);
            AnimationController.AddAnimation("attack1", content.Load<Texture2D>("PlayerSprites/Shinobi/Attack_1"), 5, 0.1, false);

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