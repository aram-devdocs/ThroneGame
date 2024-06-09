using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Maps
{
    public class DemoMap : Map
    {
        private string _jsonFilePath;

        public DemoMap()
        {
            _jsonFilePath = "Content/Maps/DemoMap.json";
        }

        protected override void LoadTilesetTexture(ContentManager content)
        {
            // Load the tileset texture
            TilesetTexture = content.Load<Texture2D>("Maps/Nature_environment_01");
        }

        public void LoadMapContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            LoadContent(graphicsDevice, content, _jsonFilePath);
        }
    }
}