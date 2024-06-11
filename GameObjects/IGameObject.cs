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

        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }
        float Mass { get; set; } // New property to represent the mass of the object


    }
}