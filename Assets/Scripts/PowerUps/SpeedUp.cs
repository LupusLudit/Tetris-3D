using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class SpeedUp : PowerUp
    {
        public override int Id => 8;

        public override string Title => "Speed up";

        public override string Description => "The fall of the current block has been sped up";

        public SpeedUp(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDelay());
        }

        private IEnumerator ActivateDelay()
        {
            Executer.DelayMultiplier = 0.5;
            yield return new WaitForSeconds(10f);
            Executer.DelayMultiplier = 1;
        }
    }
}