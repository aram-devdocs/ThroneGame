using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Utils
{
    public static class TextureUtils
    {
        public static Texture2D CreateRedBorderTexture(GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Red });
            return texture;
        }

        public static void DebugBorder(SpriteBatch spriteBatch, int x, int y, int width, int height)
        {
            Texture2D redBorderTexture = CreateRedBorderTexture(spriteBatch.GraphicsDevice);
            // Draw the red border
            int borderWidth = 2;
            // Top border
            spriteBatch.Draw(redBorderTexture, new Rectangle(x, y, width, borderWidth), Color.White);
            // Bottom border
            spriteBatch.Draw(redBorderTexture, new Rectangle(x, y + height - borderWidth, width, borderWidth), Color.White);
            // Left border
            spriteBatch.Draw(redBorderTexture, new Rectangle(x, y, borderWidth, height), Color.White);
            // Right border
            spriteBatch.Draw(redBorderTexture, new Rectangle(x + width - borderWidth, y, borderWidth, height), Color.White);

            redBorderTexture.Dispose();
        }

        public static void DebugPosition(SpriteBatch spriteBatch, int x, int y)
        {
            //   create a white dot at the position, then draw a red border around it to make it more visible
            Texture2D whiteDot = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteDot.SetData(new Color[] { Color.White });
            spriteBatch.Draw(whiteDot, new Rectangle(x, y, 1, 1), Color.White);
            DebugBorder(spriteBatch, x, y, 1, 1);
            whiteDot.Dispose();
        }

        public static void DebugLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            // Draw a line from start to end
            Texture2D whiteDot = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteDot.SetData(new Color[] { Color.White });
            Vector2 direction = end - start;
            float distance = direction.Length();
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            spriteBatch.Draw(whiteDot, new Rectangle((int)start.X, (int)start.Y, (int)distance, 1), null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            whiteDot.Dispose();
        }
    }
}