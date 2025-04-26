using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlockFreezer"]/*'/>
    public class BlockFreezer : PowerUp
    {

        public override int Id => 6;

        public override string Title => "Block freezing";

        public override string Description => "Your current block has been temporarily freezed";

        public BlockFreezer(GameExecuter executer) : base(executer) { }


        /// <summary>
        /// Triggers the freeze powerup effect by starting a coroutine.
        /// When activated, the current block becomes frozen (cannot be moved or rotated)
        /// for a limited duration.
        /// </summary>
        public override void Use()
        {
            Executer.StartCoroutine(FreezeBlock());
        }


        /// <summary>
        /// Freezes the block for 10 seconds, preventing any movement or rotation.
        /// After the duration expires, the block is unfrozen and normal behavior resumes.
        /// </summary>
        private IEnumerator FreezeBlock()
        {
            Executer.Manager.Freezed = true;
            yield return new WaitForSeconds(10f);
            Executer.Manager.Freezed = false;
        }
    }
}
