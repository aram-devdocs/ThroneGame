using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThroneGame.Tiles;

namespace ThroneGame.Maps
{
    public interface IMap
    {
        void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager);
        void Draw(SpriteBatch spriteBatch);


        string _jsonFilePath { get; set; }

        List<ITile> Tiles { get; set; }
        Texture2D TilesetTexture { get; set; }

    }
}