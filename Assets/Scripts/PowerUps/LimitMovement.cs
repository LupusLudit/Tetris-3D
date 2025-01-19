using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class LimitMovement : PowerUp
    {
        public override int Id => 11;

        public override string Title => "Movement limitation";

        public override string Description => "You can only move down and rotate around your axis";

        public LimitMovement(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateMovementLimitation());
        }

        private IEnumerator ActivateMovementLimitation()
        {
            Executer.Manager.LimitedMovement = true;
            yield return new WaitForSeconds(10f);
            Executer.Manager.LimitedMovement = false;
        }
    }
}
