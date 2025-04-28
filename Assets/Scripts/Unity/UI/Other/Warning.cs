using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.UI.Other
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Warning"]/*'/>
    public class Warning : MonoBehaviour
    {
        public GameObject WarningPanel;
        public GameExecuter Executer;

        private Image warningImage;
        public float BlinkSpeed = 4f;

        /*
         * Universal constant and variable
         * They can be assigned a value in specific classes (game mods)
         * Example Use:
         * Universal Constant = 10 (10 blocks)
         * Universal Variable = Blocks remaining
         * => If Blocks remaining < 10, show warning
        */
        public int UniversalConstant { get; set; } = 0;
        public int UniversalVariable { get; set; } = 0;
        private bool canDraw = false;

        void Start()
        {
            // Finds the Image component inside the UI Panel
            warningImage = WarningPanel.GetComponentInChildren<Image>();
        }

        /// <summary>
        /// Updates the warning panel based on game activity and specific game conditions.
        /// </summary>
        void Update()
        {
            if (Executer.IsGameActive())
            {
                if (!canDraw && Executer.BlocksPlaced == 0) canDraw = true;
                else if (NotEnough())
                {
                    WarningPanel.SetActive(true);
                    Blink();
                }
                else if (BlocksNearTop() && !NotEnough())
                {
                    StartCoroutine(ShowOnce());
                }
            }
            else WarningPanel.SetActive(false);
        }

        /// <summary>
        /// Coroutine that briefly shows the warning panel with full opacity and then hides it after a short delay.
        /// </summary>
        /// <returns>IEnumerator for coroutine handling.</returns>
        public IEnumerator ShowOnce()
        {
            WarningPanel.SetActive(true);
            Color color = warningImage.color;
            color.a = 1;
            warningImage.color = color;
            yield return new WaitForSeconds(0.5f);
            WarningPanel.SetActive(false);
        }

        /// <summary>
        /// Checks if blocks are reaching near the top of the game grid.
        /// </summary>
        /// <returns><c>true</c> if the top layers are occupied and blocks have been placed. Otherwise, <c>false</c>.</returns>
        private bool BlocksNearTop()
        {
            return !Executer.CurrentGame.Grid.IsLayerEmpty(Executer.YMax - 3)
                && Executer.BlocksPlaced > 0 && canDraw;
        }

        /// <summary>
        /// Checks if the universal variable falls below the defined universal constant.
        /// </summary>
        /// <returns><c>true</c> if a warning should be triggered based on resource shortage. Otherwise, <c>false</c>.</returns>
        private bool NotEnough()
        {
            return UniversalVariable < UniversalConstant;
        }

        /// <summary>
        /// Makes the warning image blink by adjusting its transparency over time.
        /// </summary>
        private void Blink()
        {
            if (warningImage != null)
            {
                float alpha = Mathf.Sin(Time.time * BlinkSpeed); // Normalize to 0-1
                Color color = warningImage.color;
                color.a = alpha;
                warningImage.color = color;
            }
        }
    }
}
