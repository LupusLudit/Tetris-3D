using System.Collections;
using UnityEngine;

public class BlitzTimer : MonoBehaviour
{
    public GameExecuter Executer;
    public TimeUI TimeUI;
    public int countdownTime = 120;

    private bool countingDown = false;

    void Start()
    {
    }
    void Update()
    {
        if (Executer.IsGameActive() && !countingDown)
        {
            StartCoroutine(StartCountdown());
            countingDown = true;
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
