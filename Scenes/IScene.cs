using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Controllers;
using ThroneGame.Entities;
using ThroneGame.Maps;

namespace ThroneGame.Scenes
{
    /// <summary>
    /// Defines the basic properties and methods for a game scene.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Gets or sets the game instance associated with this scene.
        /// </summary>
        Game1 Game { get; set; }

        /// <summary>
        /// Gets or sets the player entity in the scene.
        /// </summary>
        PlayerEntity Player { get; set; }

        /// <summary>
        /// Gets or sets the camera controller for the scene.
        /// </summary>
        CameraController CameraController { get; set; }

        /// <summary>
        /// Gets or sets the physics controller for the scene.
        /// </summary>
        PhysicsController PhysicsController { get; set; }

        /// <summary>
        /// Gets or sets the background image for the scene.
        /// </summary>
        Texture2D BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the map of the scene.
        /// </summary>
        IMap Map { get; set; }

        /// <summary>
        /// Gets or sets the list of entities in the scene.
        /// </summary>
        List<IEntity> Entities { get; set; }

        /// <summary>
        /// Loads the content required for the scene.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Updates the scene based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the scene using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Resets the scene to its initial state.
        /// </summary>
        /// <param name="game1">The game instance.</param>
        /// <param name="gameTime">The game time information.</param>
        void Reset(Game1 game1, GameTime gameTime);
    }
}