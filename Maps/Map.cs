using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ThroneGame.Tiles;

namespace ThroneGame.Maps
{
    /// <summary>
    /// Represents a game map composed of tiles.
    /// </summary>
    public abstract class Map : IMap
    {
        /// <summary>
        /// Gets or sets the list of tiles that make up the map.
        /// </summary>
        public List<ITile> Tiles { get; set; }

        /// <summary>
        /// Gets or sets the texture for the tileset used in the map.
        /// </summary>
        public Texture2D TilesetTexture { get; set; }

        /// <summary>
        /// Gets or sets the file path to the JSON file containing the map data.
        /// </summary>
        public string JsonFilePath { get; set; }

        protected int TileWidth;
        protected int TileHeight;
        protected int TilesetColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            Tiles = new List<ITile>();
        }

        /// <summary>
        /// Loads the content for the map, including tileset textures and other resources.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used for rendering.</param>
        /// <param name="contentManager">The content manager used for loading resources.</param>
        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            LoadContent(graphicsDevice, contentManager, JsonFilePath);
        }

        /// <summary>
        /// Loads the content for the map from a JSON file.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used for rendering.</param>
        /// <param name="contentManager">The content manager used for loading resources.</param>
        /// <param name="jsonFilePath">The file path to the JSON file containing the map data.</param>
        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager, string jsonFilePath)
        {
            JsonFilePath = jsonFilePath;
            LoadTilesFromJson(graphicsDevice, jsonFilePath);
        }

        /// <summary>
        /// Draws the map using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle visibleArea)
        {
            foreach (var tile in Tiles)
            {
                if (tile.Bounds.Intersects(visibleArea))
                {
                    tile.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Loads tiles from a JSON file.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device used for rendering.</param>
        /// <param name="jsonFilePath">The file path to the JSON file containing the map data.</param>
        protected void LoadTilesFromJson(GraphicsDevice graphicsDevice, string jsonFilePath)
        {
            var jsonContent = File.ReadAllText(jsonFilePath);
            var mapData = JsonConvert.DeserializeObject<MapData>(jsonContent);

            if (mapData == null || mapData.Layers == null || mapData.Layers.Count == 0)
            {
                throw new InvalidDataException("Invalid JSON format or missing layers data.");
            }

            TileWidth = mapData.TileWidth;
            TileHeight = mapData.TileHeight;
            TilesetColumns = TilesetTexture.Width / TileWidth;

            var layer = mapData.Layers[0]; // Assuming a single layer for simplicity
            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    int tileId = layer.Data[y * layer.Width + x];
                    if (tileId > 0) // Assuming 0 means no tile
                    {
                        var position = new Vector2(x * TileWidth, y * TileHeight);
                        var tileSourceRectangle = GetTileSourceRectangle(tileId);
                        var tile = new Tile(TilesetTexture, tileSourceRectangle, true, position, TileWidth, TileHeight);
                        Tiles.Add(tile);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the source rectangle for a tile based on its ID.
        /// </summary>
        /// <param name="tileId">The ID of the tile.</param>
        /// <returns>The source rectangle for the tile.</returns>
        private Rectangle GetTileSourceRectangle(int tileId)
        {
            int tileIndex = tileId - 1; // Assuming tile IDs start from 1
            int tileX = tileIndex % TilesetColumns * TileWidth;
            int tileY = tileIndex / TilesetColumns * TileHeight;
            return new Rectangle(tileX, tileY, TileWidth, TileHeight);
        }

        /// <summary>
        /// Represents the data structure of the map read from the JSON file.
        /// </summary>
        private class MapData
        {
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public List<LayerData> Layers { get; set; }
        }

        /// <summary>
        /// Represents the data structure of a layer in the map.
        /// </summary>
        private class LayerData
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}