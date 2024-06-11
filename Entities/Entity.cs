using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Utils;

namespace ThroneGame.Entities
{
    public abstract class Entity : IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsCollidable { get; set; }
        public bool IsOnGround { get; set; }
        public float HorizontalBoundsPadding { get; set; }
        public float VerticalBoundsPadding { get; set; }


        public AnimationController AnimationController { get; private set; }

        public int FrameWidth => AnimationController.FrameWidth;
        public int FrameHeight => AnimationController.FrameHeight;
        public Rectangle Bounds { get; set; }
        public bool IsFacingRight
        {
            get => AnimationController.IsFacingRight;
            set => AnimationController.IsFacingRight = value;
        }

        public MovementController MovementController { get; set; }

        public Entity(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            IsCollidable = true;
            IsOnGround = false;
            HorizontalBoundsPadding = 1;
            VerticalBoundsPadding = 1;

            MovementController = new MovementController();
            AnimationController = new AnimationController();
        }

        public virtual void Update(GameTime gameTime)
        {
            // Handle movement
            MovementController?.HandleMovement(this, gameTime);

            // Update animation
            AnimationController.Update(gameTime);

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // TODO - Clean up vertical bounds padding
            Bounds = new Rectangle((int)(Position.X - (HorizontalBoundsPadding / 2)), (int)(Position.Y - (VerticalBoundsPadding / 2)), (int)(FrameWidth + HorizontalBoundsPadding), (int)(FrameHeight + VerticalBoundsPadding)); 
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            AnimationController.Draw(spriteBatch, Position);
        }

        public void Destroy()
        {
            // TODO: Implement
        }
    }
}
