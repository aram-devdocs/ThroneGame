using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Maps
{
    /// <summary>
    /// Represents a demo map in the game.
    /// </summary>
    public class HomeMap : Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeMap"/> class with the specified content manager.
        /// </summary>
        /// <param name="content">The content manager used for loading textures.</param>
        public HomeMap(ContentManager content)
        {
            TilesetTexture = content.Load<Texture2D>("Maps/home_tileset_compact");
            JsonFilePath = "Content/Maps/Home.json";
            BackgroundImage = content.Load<Texture2D>("Backgrounds/1");
        }


    }
}