using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    public class SceneManager
    {
        private Dictionary<string, IScene> _scenes;
        private IScene _currentScene;

        public SceneManager()
        {
            _scenes = new Dictionary<string, IScene>();
        }

        public void AddScene(string name, IScene scene)
        {
            _scenes[name] = scene;
        }

        public void SetCurrentScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                _currentScene = _scenes[name];
                _currentScene.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }

        public void ResetCurrentScene(Game1 game1)
        {
            _currentScene?.Reset(game1);
        }


    }
}