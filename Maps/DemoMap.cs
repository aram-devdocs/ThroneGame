using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Maps
{
    /// <summary>
    /// Represents a demo map in the game.
    /// </summary>
    public class DemoMap : Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoMap"/> class with the specified content manager.
        /// </summary>
        /// <param name="content">The content manager used for loading textures.</param>
        public DemoMap(ContentManager content)
        {
            TilesetTexture = content.Load<Texture2D>("Maps/Nature_environment_01");
            JsonFilePath = "Content/Maps/DemoMapWide.json";
            CollisionLayerIndex = new int[] { 0, 1 };
            BackgroundImage = content.Load<Texture2D>("Backgrounds/1");
        }


    }
}