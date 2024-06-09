using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public interface IScene
    {
        Texture2D BackgroundImage { get; set; }
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}