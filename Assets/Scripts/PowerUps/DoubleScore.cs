using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="DoubleScore"]/*'/>
    public class DoubleScore : PowerUp
    {
        public override int Id => 5;

        public override string Title => "Double score";

        public override string Description => "For a few seconds, your score earnings double";

        public DoubleScore(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Starts a coroutine that temporarily doubles all score earnings.
        /// </summary>
        public override void Use()
        {
            Executer.StartCoroutine(ActivateDoubleScore());
        }

        /// <summary>
        /// Doubles the score earnings for 10 seconds, then resets to normal.
        /// </summary>
        private IEnumerator ActivateDoubleScore()
        {
            Executer.Manager.DoubleScore = true;
            yield return new WaitForSeconds(10f);
            Executer.Manager.DoubleScore = false;
        }
    }
}
