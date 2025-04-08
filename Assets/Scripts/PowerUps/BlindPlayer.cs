using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class BlindPlayer : PowerUp
    {
        private GameObject blindMaskPanel;

        public BlindPlayer(GameExecuter executer) : base(executer) {}

        public override int Id => 9;

        public override string Title => "Blinded by the lights";

        public override string Description => "You have been temporarily blinded";

        public override void Use()
        {
            if (blindMaskPanel == null)
            {
                var parentObject = GameObject.Find("Canvas");
                blindMaskPanel = parentObject.transform.Find("BlindingOverlay")?.gameObject;
            }

            Executer.StartCoroutine(BlindEffectCoroutine());
        }

        private IEnumerator BlindEffectCoroutine()
        {
            blindMaskPanel.SetActive(true);
            yield return new WaitForSeconds(10);
            blindMaskPanel.SetActive(false);
        }
    }
}
