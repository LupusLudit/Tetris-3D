using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timer;

    public void UpdateTime(string message)
    {
        timer.text = message;
    }
}
