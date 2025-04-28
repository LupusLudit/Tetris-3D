using Assets.Scripts.Unity.UI.DynamicMessages;
using Assets.Scripts.Unity.UI.Other;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Unity
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlitzTimer"]/*'/>
    public class BlitzTimer : MonoBehaviour
    {
        public GameExecuter Executer;
        public DynamicMessage Timer;
        public PopUpMessage TimePlus;
        public Warning Warning;
        public int CountdownTime = 180;

        private bool countingDown = false;
        // Executer.Manager.ClearedLayers can be true for multiple turns, therefor we need to add an extra bool.
        private bool canAddTime = false;

        private void Start()
        {
            Warning.UniversalConstant = 10;
        }

        /// <summary>
        /// Updates the timer state every frame, starts countdown, 
        /// and adds extra time when conditions are met.
        /// </summary>
        void Update()
        {
            if (Executer.IsGameActive() && !countingDown)
            {
                StartCoroutine(StartCountdown());
                countingDown = true;
            }

            //Adding time if the player cleared some layers
            if (!canAddTime && Executer.Manager.ClearedLayers == 0) canAddTime = true;
            else if (Executer.Manager.ClearedLayers > 0 && canAddTime)
            {
                int extraTime = Executer.Manager.ClearedLayers * 20;
                AddTime(extraTime);
            }
            Warning.UniversalVariable = CountdownTime;
        }

        /// <summary>
        /// Displays a popup UI message indicating how much extra time was added.
        /// </summary>
        /// <param name="extraTime">Amount of extra time added in seconds.</param>
        private void DisplayUIMessage(int extraTime) => TimePlus.DisplayUpdatedText($"+ {extraTime} sec");

        /// <summary>
        /// Adds extra time to the countdown and displays a message.
        /// </summary>
        /// <param name="extraTime">Amount of extra time in seconds.</param>
        private void AddTime(int extraTime)
        {
            CountdownTime += extraTime;
            DisplayUIMessage(extraTime);
            canAddTime = false;
        }

        /// <summary>
        /// Handles the countdown logic, updating the UI and triggering game over when time expires.
        /// </summary>
        IEnumerator StartCountdown()
        {
            while (CountdownTime > 0)
            {
                Timer.UpdateMessage($"Time remaining: {CountdownTime} sec");

                yield return new WaitForSeconds(1f);
                if (!Executer.UI.GameMenu.IsPaused && !Executer.CurrentGame.GameOver) CountdownTime--;
            }

            Timer.UpdateMessage("Times up!");
            Executer.CurrentGame.GameOver = true;
        }
    }
}