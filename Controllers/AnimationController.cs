using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Represents a single animation with its properties and frames.
    /// </summary>
    public class Animation
    {
        public Texture2D Texture { get; }
        public int FrameCount { get; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public double FrameDuration { get; }
        public bool IsLooping { get; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="texture">The texture containing the animation frames.</param>
        /// <param name="frameCount">The number of frames in the animation.</param>
        /// <param name="frameDuration">The duration of each frame in seconds.</param>
        /// <param name="isLooping">Whether the animation should loop.</param>
        public Animation(Texture2D texture, int frameCount, double frameDuration, bool isLooping = true)
        {
            Texture = texture;
            FrameCount = frameCount;
            FrameWidth = texture.Width / frameCount;
            FrameHeight = texture.Height;
            FrameDuration = frameDuration;
            IsLooping = isLooping;

        }
    }

    /// <summary>
    /// Controls the animation states and frame updates for a game entity.
    /// </summary>
    public class AnimationController
    {
        private readonly Dictionary<string, Animation> _animations;
        private string _currentState;
        private int _currentFrame;
        private double _timeCounter;
        private Rectangle _sourceRectangle;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is facing right.
        /// </summary>
        public bool IsFacingRight { get; set; }

        /// <summary>
        /// Gets the width of the current animation frame.
        /// </summary>
        public int FrameWidth => _animations.ContainsKey(_currentState) ? _animations[_currentState].FrameWidth : 0;

        /// <summary>
        /// Gets the height of the current animation frame.
        /// </summary>
        public int FrameHeight => _animations.ContainsKey(_currentState) ? _animations[_currentState].FrameHeight : 0;


        public bool IsAttacking { get; set; }
        public double AttackEndTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationController"/> class.
        /// </summary>
        public AnimationController()
        {
            _animations = new Dictionary<string, Animation>();
            _currentFrame = 0;
            _timeCounter = 0;
            IsFacingRight = true; // Default facing direction
        }

        /// <summary>
        /// Adds a new animation state.
        /// </summary>
        /// <param name="state">The name of the animation state.</param>
        /// <param name="texture">The texture containing the animation frames.</param>
        /// <param name="frameCount">The number of frames in the animation.</param>
        /// <param name="frameDuration">The duration of each frame in seconds.</param>
        /// <param name="isLooping">Whether the animation should loop.</param>
        public void AddAnimation(string state, Texture2D texture, int frameCount, double frameDuration, bool isLooping = true)

        {
            var animation = new Animation(texture, frameCount, frameDuration, isLooping);
            _animations[state] = animation;



            if (_currentState == null)
            {
                SetState(state);
            }
        }

        /// <summary>
        /// Sets the current animation state.
        /// </summary>
        /// <param name="state">The name of the animation state to set.</param>
        public void SetState(string state)
        {
            if (_animations.ContainsKey(state) && state != _currentState)
            {
                _currentFrame = 0;
                _timeCounter = 0;
                _currentState = state;
                UpdateSourceRectangle();
            }
        }

        /// <summary>
        /// Updates the animation frame based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {
            if (_currentState == null) return;

            var animation = _animations[_currentState];
            _timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeCounter >= animation.FrameDuration)
            {
                if (animation.IsLooping)
                {
                    _currentFrame = (_currentFrame + 1) % animation.FrameCount;
                }
                else
                {
                    _currentFrame = Math.Min(_currentFrame + 1, animation.FrameCount - 1);
                }
                UpdateSourceRectangle();
                _timeCounter -= animation.FrameDuration;
            }

            if (IsAttacking && _timeCounter >= AttackEndTime)
            {
                //  Set is attacking to false
                IsAttacking = false;
            }
        }

        /// <summary>
        /// Draws the current animation frame at the specified position.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        /// <param name="position">The position to draw the animation frame at.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_currentState == null) return;

            var animation = _animations[_currentState];
            var flipEffect = IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(animation.Texture, position, _sourceRectangle, Color.White, 0, Vector2.Zero, 1, flipEffect, 0);
        }

        /// <summary>
        /// Updates the source rectangle for the current animation frame.
        /// </summary>
        private void UpdateSourceRectangle()
        {
            var animation = _animations[_currentState];
            _sourceRectangle = new Rectangle(_currentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight);
        }
    }
}