using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{

    public GameObject CountdownPanel;
    public TextMeshProUGUI CountdownText;

    public IEnumerator StartCounting()
    {
        CountdownText.gameObject.SetActive(true);
        CountdownText.text = "GET READY!";
        yield return new WaitForSeconds(1f);

        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        CountdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        CountdownText.gameObject.SetActive(false);
        CountdownPanel.SetActive(false);
    }
}
