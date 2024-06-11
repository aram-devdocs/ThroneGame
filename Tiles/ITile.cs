using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame;

namespace ThroneGame.Tiles
{
    /// <summary>
    /// Defines the basic properties and methods for a tile in the game.
    /// </summary>
    public interface ITile : IGameObject
    {
        /// <summary>
        /// Gets or sets the width of the tile.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the tile.
        /// </summary>
        int Height { get; set; }
    }
}