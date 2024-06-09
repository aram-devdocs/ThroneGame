using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;

namespace ThroneGame.Entities
{
    public abstract class Entity : IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsCollidable { get; set; }
        public bool IsOnGround { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public string State { get; set; }
        public int FrameCount { get; set; }
        public int CurrentFrame { get; set; }
        public double FrameTime { get; set; }
        public double TimeCounter { get; set; }

        public MovementController MovementController { get; set; }

        public Entity(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount)
        {
            Texture = texture;
            Position = position;
            Velocity = Vector2.Zero;
            IsCollidable = true;
            IsOnGround = false;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameCount = frameCount;
            CurrentFrame = 0;
            FrameTime = 0.1; // Change frame every 0.1 seconds
            TimeCounter = 0;
            State = "idle";
            SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);

            MovementController = new MovementController();
        }

        public virtual void Update(GameTime gameTime)
        {
            // Handle movement
            MovementController?.HandleMovement(this, gameTime);

            // Update animation frame
            TimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeCounter >= FrameTime)
            {
                CurrentFrame = (CurrentFrame + 1) % FrameCount;
                SourceRectangle = new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
                TimeCounter -= FrameTime;
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White);
        }
    }
}