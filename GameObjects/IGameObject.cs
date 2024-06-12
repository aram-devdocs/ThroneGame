using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame
{
    /// <summary>
    /// Defines the basic properties and methods for a game object.
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Updates the game object's state.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the game object using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Destroys the game object.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Gets or sets a value indicating whether the game object is collidable.
        /// </summary>
        bool IsCollidable { get; set; }

        /// <summary>
        /// Gets or sets the bounds of the game object.
        /// </summary>
        Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the position of the game object.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the game object.
        /// </summary>
        Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the mass of the game object.
        /// </summary>
        float Mass { get; set; }

        /// <summary>
        /// Gets or sets the vertices of the game object.
        /// </summary>
        Vector2[] Vertices { get; set; }
    }
}