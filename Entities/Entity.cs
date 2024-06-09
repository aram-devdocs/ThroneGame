using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;

namespace ThroneGame.Entities
{
    public class Animation
    {
        public Texture2D AnimationTexture { get; set; }
        public int FrameCount { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public double FrameTime { get; set; }
        public bool Looping { get; set; }

        public Animation(Texture2D texture, int frameCount, double frameTime, bool looping = true)
        {
            AnimationTexture = texture;
            FrameCount = frameCount;
            FrameWidth = texture.Width / frameCount;
            FrameHeight = texture.Height;
            FrameTime = frameTime;
            Looping = looping;
        }

        public void SetLooping(bool looping)
        {
            Looping = looping;
        }
    }
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

        public bool IsFacingRight { get; set; }
        public int FrameCount { get; set; }
        public int CurrentFrame { get; set; }
        public double FrameTime { get; set; }
        public double TimeCounter { get; set; }

        public MovementController MovementController { get; set; }

        private Dictionary<string, Animation> _animations;
        private string _currentState;

        public Entity(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            IsCollidable = true;
            IsOnGround = false;
            FrameWidth = 0;
            FrameHeight = 0;
            FrameCount = 0;
            CurrentFrame = 0;
            FrameTime = 0.1; // Change frame every 0.1 seconds
            TimeCounter = 0;
            State = "run";
            IsFacingRight = true;
            SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);

            MovementController = new MovementController();
            _animations = new Dictionary<string, Animation>();
        }

        public void AddAnimation(string state, Texture2D texture, int frameCount, double frameTime, bool looping = true)
        {
            var animation = new Animation(texture, frameCount, frameTime, looping);
            _animations[state] = animation;

            if (_currentState == null)
            {
                SetState(state);
            }
        }


        public void SetState(string state)
        {
            if (_animations.ContainsKey(state))
            {
                _currentState = state;
                var animation = _animations[state];
                Texture = animation.AnimationTexture;
                FrameWidth = animation.FrameWidth;
                FrameHeight = animation.FrameHeight;
                FrameCount = animation.FrameCount;
                FrameTime = animation.FrameTime;
            }
        }


        public virtual void Update(GameTime gameTime)
        {
            // Handle movement
            MovementController?.HandleMovement(this, gameTime);

            // Update animation frame
            TimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeCounter >= FrameTime)
            {
                if (_animations[_currentState].Looping)
                {
                    CurrentFrame = (CurrentFrame + 1) % FrameCount;
                }
                else
                {
                    CurrentFrame = Math.Min(CurrentFrame + 1, FrameCount - 1);
                }
                SourceRectangle = new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
                TimeCounter -= FrameTime;
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var effects = IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White, 0, Vector2.Zero, 1, effects, 0);
        }
    }
}