using Assets.Scripts.Unity;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUp"]/*'/>
    public abstract class PowerUp
    {
        // Note: Commentary for this class implies for all powerup overrides as well.

        public Vector3 Position { get; set; }
        public abstract int Id { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        protected GameExecuter Executer { get; }
        public PowerUp(GameExecuter executer)
        {
            Executer = executer;
        }

        /// <summary>
        /// Executes the specific effect of the powerup when it is activated.
        /// Defines the powerup's behavior.
        /// </summary>
        public abstract void Use();
    }
}

