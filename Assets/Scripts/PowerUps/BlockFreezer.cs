using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class BlockFreezer : PowerUp
    {

        public override int Id => 6;
        public BlockFreezer(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDoubleScore());
        }

        private IEnumerator ActivateDoubleScore()
        {
            Executer.Freezed = true;
            yield return new WaitForSeconds(10f); //Temporarily set to 10
            Executer.Freezed = false;
        }
    }
}
