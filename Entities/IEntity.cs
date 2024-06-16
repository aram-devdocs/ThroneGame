using Microsoft.Xna.Framework;

namespace ThroneGame.Entities
{
    /// <summary>
    /// Defines the basic properties and methods for a game entity.
    /// </summary>
    public interface IEntity : IGameObject
    {


        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is on the ground.
        /// </summary>
        bool IsOnGround { get; set; }

        /// <summary>
        /// Gets the width of the current animation frame.
        /// </summary>
        int FrameWidth { get; }

        /// <summary>
        /// Gets the height of the current animation frame.
        /// </summary>
        int FrameHeight { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is facing right.
        /// </summary>
        bool IsFacingRight { get; set; }

        /// <summary>
        /// Is the  entity attacking.
        /// </summary>
        bool IsAttacking { get; set; }

        /// <summary>
        /// Scale of the entity and its bounds
        /// </summary>
        float Scale { get; set; }



        /// <summary>
        /// The time in seconds that the attack started, to ensure we only attack once per cycle
        /// </summary>
        double AttackEndTime { get; set; }
    }
}