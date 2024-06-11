using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThroneGame.Scenes
{
    /// <summary>
    /// Manages scenes within the game, allowing for switching between scenes and updating/drawing the current scene.
    /// </summary>
    public class SceneManager
    {
        private readonly Dictionary<string, IScene> _scenes;
        private IScene _currentScene;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneManager"/> class.
        /// </summary>
        public SceneManager()
        {
            _scenes = new Dictionary<string, IScene>();
        }

        /// <summary>
        /// Adds a scene to the scene manager.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="scene">The scene to add.</param>
        public void AddScene(string name, IScene scene)
        {
            _scenes[name] = scene;
        }

        /// <summary>
        /// Sets the current scene by name.
        /// </summary>
        /// <param name="name">The name of the scene to set as current.</param>
        public void SetCurrentScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                _currentScene = _scenes[name];
                _currentScene.LoadContent();
            }
        }

        /// <summary>
        /// Updates the current scene.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        /// <summary>
        /// Draws the current scene.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }

        /// <summary>
        /// Resets the current scene.
        /// </summary>
        /// <param name="game1">The game instance.</param>
        /// <param name="gameTime">The game time information.</param>
        public void ResetCurrentScene(Game1 game1, GameTime gameTime)
        {
            _currentScene?.Reset(game1, gameTime);
        }
    }
}