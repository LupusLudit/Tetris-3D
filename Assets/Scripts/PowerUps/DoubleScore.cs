using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class DoubleScore : PowerUp
    {
        public override int Id => 5;

        public override string Title => "Double score";

        public override string Description => "For a few seconds, your score earnings double";

        public DoubleScore(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.StartCoroutine(ActivateDoubleScore());
        }

        private IEnumerator ActivateDoubleScore()
        {
            Executer.Manager.DoubleScore = true;
            yield return new WaitForSeconds(10f); //Temporarily set to 10
            Executer.Manager.DoubleScore = false;
        }
    }
}
