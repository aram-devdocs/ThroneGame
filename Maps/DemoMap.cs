using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Maps
{
    public class DemoMap : Map
    {

        public DemoMap(ContentManager content)
        {
            this.JsonFilePath = "Content/Maps/DemoMap.json";
            // Load the tileset texture
            this.TilesetTexture = content.Load<Texture2D>("Maps/Nature_environment_01");
        }


    }
}