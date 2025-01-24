using System.Collections;
using UnityEngine;

public class BlitzTimer : MonoBehaviour
{
    public GameExecuter Executer;
    public TimerUI Timer;
    public TimePlus TimePlusUI;
    public int countdownTime = 120;

    private bool countingDown = false;
    private bool addTime = false;

    void Update()
    {
        if (Executer.IsGameActive() && !countingDown)
        {
            StartCoroutine(StartCountdown());
            countingDown = true;
        }

        //Adding time if the player cleared some layers
        if (Executer.Manager.ClearedLayers == 0) addTime = true;
        else if (Executer.Manager.ClearedLayers > 0 && addTime)
        {
            int extraTime = Executer.Manager.ClearedLayers * 5;
            countdownTime += extraTime;
            DisplayUIMessage(extraTime);
            addTime = false;
        }
    }

    private void DisplayUIMessage(int extraTime) =>
        TimePlusUI.DisplayTimeMessage($"+ {extraTime} sec");

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            Timer.UpdateTime($"Time remaining: {countdownTime}");

            yield return new WaitForSeconds(1f);
            if(!Executer.UI.GameMenu.IsPaused) countdownTime--;
        }

        Timer.UpdateTime("Times up!");
        Executer.CurrentGame.GameOver = true;
    }
}
