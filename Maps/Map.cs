using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ThroneGame.Tiles;

namespace ThroneGame.Maps
{
    public abstract class Map : IMap
    {
        public List<ITile> Tiles { get; set; }
        public Texture2D TilesetTexture { get; set; }
        protected int TileWidth;
        protected int TileHeight;
        protected int TilesetColumns;
        public string _jsonFilePath { get; set; }

        public Map()
        {
            Tiles = new List<ITile>();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string jsonFilePath)
        {
            LoadTilesFromJson(graphicsDevice, jsonFilePath);
        }


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

        private Rectangle GetTileSourceRectangle(int tileId)
        {
            int tileIndex = tileId - 1; // Assuming tile IDs start from 1
            int tileX = tileIndex % TilesetColumns * TileWidth;
            int tileY = tileIndex / TilesetColumns * TileHeight;
            return new Rectangle(tileX, tileY, TileWidth, TileHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in Tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        private class MapData
        {
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public List<LayerData> Layers { get; set; }
        }

        private class LayerData
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            LoadContent(graphicsDevice, content, _jsonFilePath);
        }


      
    }
}