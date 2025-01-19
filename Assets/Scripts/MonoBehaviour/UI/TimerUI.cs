using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timer;

    public void UpdateTime(string message)
    {
        timer.text = message;
    }
}
