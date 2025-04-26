using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="LimitMovement"]/*'/>
    public class LimitMovement : PowerUp
    {
        public override int Id => 11;

        public override string Title => "Movement limitation";

        public override string Description => "You can only move down and rotate around your axis";

        public LimitMovement(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Starts a coroutine that temporarily limits the player's movement controls.
        /// </summary>
        public override void Use()
        {
            Executer.StartCoroutine(ActivateMovementLimitation());
        }

        /// <summary>
        /// Limits the player's movement for 10 seconds, then restores full control.
        /// </summary>
        private IEnumerator ActivateMovementLimitation()
        {
            Executer.Manager.LimitedMovement = true;
            yield return new WaitForSeconds(10f);
            Executer.Manager.LimitedMovement = false;
        }
    }
}
