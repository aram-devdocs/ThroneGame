using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame
{
    public interface IGameObject
    {

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Destroy();

        bool IsCollidable { get; set; }


        Rectangle Bounds { get; set; }

        float HorizontalBoundsPadding { get; set; }
        float VerticalBoundsPadding { get; set; }

        Vector2 Position { get; set; }


    }
}