using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Controllers
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
    }

    public class AnimationController
    {
        private Dictionary<string, Animation> _animations;
        private string _currentState;
        private int _currentFrame;
        private double _timeCounter { get; set; }
        private Rectangle _sourceRectangle;
        public bool IsFacingRight { get; set; }

        public AnimationController()
        {
            _animations = new Dictionary<string, Animation>();
            _currentFrame = 0;
            _timeCounter = 0;
            IsFacingRight = true; // Default facing direction
        }

        public int FrameWidth => _animations.ContainsKey(_currentState) ? _animations[_currentState].FrameWidth : 0;
        public int FrameHeight => _animations.ContainsKey(_currentState) ? _animations[_currentState].FrameHeight : 0;

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
                UpdateSourceRectangle();

                if (state != _currentState)
                {
                    _currentFrame = 0;
                    _timeCounter = 0;
                }
            }
        }
        public void Update(GameTime gameTime)
        {


            if (_currentState == null) return;

            var animation = _animations[_currentState];



            _timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeCounter >= animation.FrameTime)
            {
                if (animation.Looping)
                {
                    _currentFrame = (_currentFrame + 1) % animation.FrameCount;
                }
                else
                {
                    _currentFrame = Math.Min(_currentFrame + 1, animation.FrameCount - 1);
                }
                UpdateSourceRectangle();
                _timeCounter -= animation.FrameTime;

            }
        }

        private void UpdateSourceRectangle()
        {
            var animation = _animations[_currentState];
            _sourceRectangle = new Rectangle(_currentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight);

        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            if (_currentState == null) return;

            var animation = _animations[_currentState];
            SpriteEffects flipEffect = IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            spriteBatch.Draw(animation.AnimationTexture, position, _sourceRectangle, Color.White, 0, Vector2.Zero, 1, flipEffect, 0);
        }


    }
}