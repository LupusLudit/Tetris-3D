using Assets.Scripts.Unity.UI.DynamicMessages;
using Assets.Scripts.Unity.UI.Other;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Unity
{

    public class BlitzTimer : MonoBehaviour
    {
        public GameExecuter Executer;
        public DynamicMessage Timer;
        public PopUpMessage TimePlus;
        public Warning Warning;
        public int CountdownTime = 180;

        private bool countingDown = false;
        //Executer.Manager.ClearedLayers can be true for multiple turns, therefor we need to add an extra bool.
        private bool canAddTime = false;

        private void Start()
        {
            Warning.UniversalConstant = 10;
        }

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

        private void DisplayUIMessage(int extraTime) => TimePlus.DisplayUpdatedText($"+ {extraTime} sec");


        private void AddTime(int extraTime)
        {
            CountdownTime += extraTime;
            DisplayUIMessage(extraTime);
            canAddTime = false;
        }
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