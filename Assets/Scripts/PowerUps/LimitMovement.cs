using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class LimitMovement : PowerUp
    {
        public override int Id => 11;

        public LimitMovement(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateMovementLimitation());
        }

        private IEnumerator ActivateMovementLimitation()
        {
            Executer.LimitedMovement = true;
            yield return new WaitForSeconds(10f);
            Executer.LimitedMovement = false;
        }
    }
}
