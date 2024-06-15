using System.Collections.Concurrent;
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
        public string JsonFilePath { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetColumns { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public Texture2D BackgroundImage { get; set; }

        // [y][x]
        private ITile[][] CollisionTileArray { get; set; }
        private RenderTarget2D _mapRenderTarget;

        // Concurrent dictionary for thread-safe caching of tiles
        private readonly ConcurrentDictionary<Vector2, ITile> _tileCache;

        public Map()
        {
            Tiles = new List<ITile>();
            _tileCache = new ConcurrentDictionary<Vector2, ITile>();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            LoadTilesFromJson(graphicsDevice, JsonFilePath);
        }

        public void DrawTileMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetMapRenderTarget(), Vector2.Zero, Color.White);
        }

        public void DrawToRenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            int mapWidth = MapWidth * TileWidth;
            int mapHeight = MapHeight * TileHeight;

            _mapRenderTarget = new RenderTarget2D(graphicsDevice, mapWidth, mapHeight);

            graphicsDevice.SetRenderTarget(_mapRenderTarget);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            foreach (var tile in Tiles)
            {
                tile.Draw(spriteBatch);
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public RenderTarget2D GetMapRenderTarget()
        {
            return _mapRenderTarget;
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
            MapWidth = mapData.Width;
            MapHeight = mapData.Height;

            CollisionTileArray = new ITile[MapHeight][];

            var layer = mapData.Layers[0];
            for (int y = 0; y < layer.Height; y++)
            {
                CollisionTileArray[y] = new ITile[layer.Width];
                for (int x = 0; x < layer.Width; x++)
                {
                    int tileId = layer.Data[y * layer.Width + x];
                    if (tileId > 0)
                    {
                        var position = new Vector2(x * TileWidth, y * TileHeight);
                        var tileSourceRectangle = GetTileSourceRectangle(tileId);
                        var tile = new Tile(TilesetTexture, tileSourceRectangle, true, position, TileWidth, TileHeight);
                        Tiles.Add(tile);

                        // Add the tile to the collision tile array
                        CollisionTileArray[y][x] = tile;
                    }
                    else
                    {
                        CollisionTileArray[y][x] = null;
                    }
                }
            }
        }

        private Rectangle GetTileSourceRectangle(int tileId)
        {
            int tileIndex = tileId - 1;
            int tileX = tileIndex % TilesetColumns * TileWidth;
            int tileY = tileIndex / TilesetColumns * TileHeight;
            return new Rectangle(tileX, tileY, TileWidth, TileHeight);
        }

        private class MapData
        {
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public List<LayerData> Layers { get; set; }
        }

        private class LayerData
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            if (BackgroundImage != null)
            {
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.Begin();
                spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
                spriteBatch.End();
            }
        }

        public ITile GetTileAtPosition(Vector2 position)
        {
            // First, try to get the tile from the cache
            if (_tileCache.TryGetValue(position, out ITile cachedTile))
            {
                return cachedTile;
            }

            // Calculate the tile coordinates
            int x = (int)(position.X / TileWidth);
            int y = (int)(position.Y / TileHeight);

            // Check if the coordinates are within bounds
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
            {
                var tile = CollisionTileArray[y][x];
                // Cache the tile for future requests
                _tileCache[position] = tile;
                return tile;
            }

            // Return null if the position is out of bounds
            return null;
        }

        public bool IsTileCollidable(Vector2 position)
        {
            var tile = GetTileAtPosition(position);
            return tile?.IsCollidable ?? false;
        }
    }
}