using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ThroneGame.Scenes
{
    public class SceneManager
    {
        private Game1 game;
        private Dictionary<string, IScene> scenes;
        private IScene currentScene;

        public SceneManager(Game1 game)
        {
            this.game = game;
            scenes = new Dictionary<string, IScene>();
        }

        public void AddScene(string name, IScene scene)
        {
            scenes[name] = scene;
        }

        public void SetScene(string name)
        {
            if (scenes.ContainsKey(name))
            {
                currentScene = scenes[name];
                currentScene.Initialize();
                currentScene.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            currentScene?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentScene?.Draw(gameTime, spriteBatch);
        }
    }
}