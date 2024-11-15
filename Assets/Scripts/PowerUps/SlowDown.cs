using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class SlowDown : PowerUp
    {
        public override int Id => 3;
        public SlowDown(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDelay());
        }

        private IEnumerator ActivateDelay()
        {
            Executer.SlowDown = true;
            yield return new WaitForSeconds(10f);
            Executer.SlowDown = false;
        }
    }
}
