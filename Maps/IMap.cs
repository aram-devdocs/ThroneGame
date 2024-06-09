using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Maps
{
    public interface IMap
    {
        void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager, string jsonFilePath);
        void Draw(SpriteBatch spriteBatch);
    }
}