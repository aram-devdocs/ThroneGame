using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Utils;

namespace ThroneGame.Entities
{
    /// <summary>
    /// Represents a base entity in the game with position, velocity, and animation.
    /// </summary>
    public abstract class Entity : IEntity
    {

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the entity.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is collidable.
        /// </summary>
        public bool IsCollidable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is on the ground.
        /// </summary>
        public bool IsOnGround { get; set; }

        /// <summary>
        /// Gets or sets the mass of the entity.
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets the animation controller for the entity.
        /// </summary>
        public AnimationController AnimationController { get; set; }


        /// <summary>
        /// Gets the width of the current animation frame.
        /// </summary>
        public int FrameWidth => AnimationController.FrameWidth;

        /// <summary>
        /// Gets the height of the current animation frame.
        /// </summary>
        public int FrameHeight => AnimationController.FrameHeight;

        /// <summary>
        /// Is the entity being attacked.
        /// </summary>
        public bool IsBeingAttacked { get; set; }

        /// <summary>
        /// If the entity is attacked, they are stunned until this time

        /// </summary>
        public double StunEndTime { get; set; }


        /// <summary>
        /// Gets or sets the bounds of the entity.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Scale of the entity and its bounds
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is facing right.
        /// </summary>
        public bool IsFacingRight
        {
            get => AnimationController.IsFacingRight;
            set => AnimationController.IsFacingRight = value;
        }

        /// <summary>
        /// Is the  entity attacking.
        /// </summary>
        public bool IsAttacking
        {
            get; set;
        }


        public double AttackEndTime { get; set; }

        /// <summary>
        /// Gets or sets the movement controller for the entity.
        /// </summary>
        public MovementController MovementController { get; set; }


        /// <summary>
        /// Gets or sets the combat controller for the entity.
        /// </summary>
        public CombatController CombatController { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class with the specified position.
        /// </summary>
        /// <param name="position">The initial position of the entity.</param>
        public Entity(string name, Vector2 position, Game1 Game)
        {
            Position = position;
            Velocity = Vector2.Zero;
            IsCollidable = true;
            IsOnGround = false;
            Mass = 1f;

            AnimationController = new AnimationController();
            CombatController = new CombatController(this, Game);
            Name = name;

        }



        /// <summary>
        /// Updates the entity's state, including handling movement and updating the animation.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public virtual void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
            UpdateBounds();
            CombatController.Update(gameTime);

            if (IsBeingAttacked && gameTime.TotalGameTime.TotalSeconds > StunEndTime)
            {
                IsBeingAttacked = false;
                AnimationController.SetState("idle");
            }
        }

        /// <summary>
        /// Draws the entity using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            AnimationController.Draw(spriteBatch, Position);
            CombatController.Draw(spriteBatch);
        }

        /// <summary>
        /// Destroys the entity. This method should be overridden by derived classes to implement destruction logic.
        /// </summary>
        public void Destroy()
        {
            // TODO: Implement destruction logic
        }

        /// <summary>
        /// Handles the entity's movement.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        private void HandleMovement(GameTime gameTime)
        {
            MovementController?.HandleMovement(this, gameTime);
        }

        /// <summary>
        /// Updates the entity's animation.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            AnimationController.Update(gameTime, this);
        }

        /// <summary>
        /// Updates the entity's position based on its velocity.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        private void UpdatePosition(GameTime gameTime)
        {
            Position += Velocity * GameUtils.GetDeltaTime(gameTime);
        }

        /// <summary>
        /// Updates the bounds of the entity.
        /// </summary>

        private void UpdateBounds()
        {


            // TODO - Inherit from IEntity and implement this method in PlayerEntity
            float topOffsetInPixels = 10f;
            float bottomOffsetInPixels = 0f;
            float leftOffsetInPixels = 0f;
            float rightOffsetInPixels = 0f;


            Bounds = new Rectangle(
                (int)Position.X + (int)leftOffsetInPixels,
                (int)Position.Y + (int)topOffsetInPixels,
                FrameWidth - (int)rightOffsetInPixels - (int)leftOffsetInPixels,
                FrameHeight - (int)bottomOffsetInPixels - (int)topOffsetInPixels
            );
        }



    }
}