using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Tiles;

namespace ThroneGame.Maps
{
    /// <summary>
    /// Defines the basic properties and methods for a game map.
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Loads the content for the map, including tileset textures and other resources.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used for rendering.</param>
        /// <param name="contentManager">The content manager used for loading resources.</param>
        void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager);

        /// <summary>
        /// Draws the map using the specified sprite batch.
        /// </summary>
        void DrawTileMap(SpriteBatch spriteBatch);
        void DrawToRenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch);


        void DrawBackground(SpriteBatch spriteBatch);
        RenderTarget2D GetMapRenderTarget();

        Texture2D BackgroundImage { get; set; }





        /// <summary>
        /// Gets or sets the file path to the JSON file containing the map data.
        /// </summary>
        string JsonFilePath { get; set; }

        /// <summary>
        /// Gets or sets the list of tiles that make up the map.
        /// </summary>
        List<ITile> Tiles { get; set; }

        /// <summary>
        /// Gets or sets the texture for the tileset used in the map.
        /// </summary>
        Texture2D TilesetTexture { get; set; }

        // int width and height

        /// <summary>
        /// Gets the width of the map in tiles. Should be calculated based on the size of the tiles and the map data.
        /// </summary>
        int MapWidth { get; }

        /// <summary>
        /// Gets the height of the map in tiles. Should be calculated based on the size of the tiles and the map data.
        /// </summary>
        int MapHeight { get; }
        int TileWidth { get; }
        int TileHeight { get; }




        ITile GetTileAtPosition(Vector2 position);


        bool IsTileCollidable(Vector2 position);



    }
}