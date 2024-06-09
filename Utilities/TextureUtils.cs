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


    }
}