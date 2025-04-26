using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="SpeedUp"]/*'/>
    public class SpeedUp : PowerUp
    {
        public override int Id => 8;

        public override string Title => "Speed up";

        public override string Description => "The fall of the current block has been sped up";

        public SpeedUp(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Starts a coroutine that temporarily increases the block's falling speed.
        /// </summary>

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDelay());
        }

        /// <summary>
        /// Speeds up the block's fall for 10 seconds (sets delay multiplier to 0.5), then restores the normal falling speed.
        /// </summary>
        private IEnumerator ActivateDelay()
        {
            Executer.DelayMultiplier = 0.5;
            yield return new WaitForSeconds(10f);
            Executer.DelayMultiplier = 1;
        }
    }
}