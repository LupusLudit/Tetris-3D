using System.Collections;
using UnityEngine;

public class BlitzTimer : MonoBehaviour
{
    public GameExecuter Executer;
    public TimeUI TimeUI;
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
        if (Executer.ClearedLayers == 0) addTime = true;
        else if (Executer.ClearedLayers > 0 && addTime)
        {
            countdownTime += Executer.ClearedLayers * 5;
            Debug.Log(Executer.ClearedLayers * 5);
            addTime = false;
        }
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            TimeUI.UpdateTime($"Time remaining: {countdownTime}");

            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        TimeUI.UpdateTime("Times up!");
        Executer.CurrentGame.GameOver = true;
    }
}
