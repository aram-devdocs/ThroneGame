using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Gets the time elapsed since the last frame.
/// </summary>
/// <param name="gameTime">The current game time.</param>
/// <returns>The time elapsed since the last frame in seconds.</returns>
namespace ThroneGame.Utils
{
    public static class GameUtils
    {

        public static float GetDeltaTime(GameTime gameTime)
        {
            // Get the time elapsed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            return deltaTime;

        }
    }
}