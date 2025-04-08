using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class BlockFreezer : PowerUp
    {

        public override int Id => 6;

        public override string Title => "Block freezing";

        public override string Description => "Your current block has been temporarily freezed";

        public BlockFreezer(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDoubleScore());
        }

        private IEnumerator ActivateDoubleScore()
        {
            Executer.Manager.Freezed = true;
            yield return new WaitForSeconds(10f); //Temporarily set to 10
            Executer.Manager.Freezed = false;
        }
    }
}
