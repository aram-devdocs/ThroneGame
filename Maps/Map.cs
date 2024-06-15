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

        public Map()
        {
            Tiles = new List<ITile>();
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


                    }

                    // if the tile is collidable, add it to the collision tile array, otherwise set it to null
                    if (tileId > 0)
                    {
                        CollisionTileArray[y][x] = new Tile(TilesetTexture, GetTileSourceRectangle(tileId), true, new Vector2(x * TileWidth, y * TileHeight), TileWidth, TileHeight);
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
            int x = (int)(position.X / TileWidth);
            int y = (int)(position.Y / TileHeight);


            try
            {
                return CollisionTileArray[y][x] ?? null;

            }
            catch (System.Exception)
            {

                return null;
            }
        }

        public bool IsTileCollidable(Vector2 position)
        {
            var tile = GetTileAtPosition(position);
            return tile?.IsCollidable ?? false;
        }
    }
}