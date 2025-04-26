using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="SlowDown"]/*'/>
    public class SlowDown : PowerUp
    {
        public override int Id => 3;

        public override string Title => "Slow down";

        public override string Description => "The fall of the current block has been slowed down";

        public SlowDown(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Starts a coroutine that temporarily reduces the block's falling speed.
        /// </summary>
        public override void Use()
        {
            Executer.StartCoroutine(ActivateDelay());
        }

        /// <summary>
        /// Slows down the block's fall for 10 seconds (sets delay multiplier to 2), then restores the normal falling speed.
        /// </summary>
        private IEnumerator ActivateDelay()
        {
            Executer.DelayMultiplier = 2;
            yield return new WaitForSeconds(10f);
            Executer.DelayMultiplier = 1;
        }
    }
}
