using Assets.Scripts.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlindPlayer"]/*'/>
    public class BlindPlayer : PowerUp
    {
        private GameObject blindMaskPanel;

        public BlindPlayer(GameExecuter executer) : base(executer) { }

        public override int Id => 9;

        public override string Title => "Blinded by the lights";

        public override string Description => "You have been temporarily blinded";


        /// <summary>
        /// Activates the blinding Overlay panel containing the
        /// "blinding mask" (an image that overlays parts of the screen, making it harder for the player to see). 
        /// </summary>
        public override void Use()
        {
            if (blindMaskPanel == null)
            {
                var parentObject = GameObject.Find("Canvas");
                blindMaskPanel = parentObject.transform.Find("BlindingOverlay")?.gameObject;
            }

            Executer.StartCoroutine(BlindEffectCoroutine());
        }

        /// <summary>
        /// For 10 seconds, the blinding mask is activated, making it difficult for the player to see.
        /// After the duration expires, the mask is deactivated.
        /// </summary>
        private IEnumerator BlindEffectCoroutine()
        {
            blindMaskPanel.SetActive(true);
            yield return new WaitForSeconds(10);
            blindMaskPanel.SetActive(false);
        }
    }
}
