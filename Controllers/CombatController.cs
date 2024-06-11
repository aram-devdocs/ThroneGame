using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThroneGame.Entities;

namespace ThroneGame.Controllers
{
    /// <summary>
    /// Controls the combat actions of an entity.
    /// </summary>
    public class CombatController
    {
        private readonly IEntity _entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatController"/> class.
        /// </summary>
        /// <param name="entity">The entity that this controller is assigned to.</param>
        public CombatController(IEntity entity)
        {
            _entity = entity;
        }

        /// <summary>
        /// Updates the combat state of the entity based on input.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        public void Update(GameTime gameTime)
        {
            
        }
    }
}