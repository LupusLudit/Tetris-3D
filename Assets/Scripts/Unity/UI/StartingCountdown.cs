using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{
    public GameObject CountdownPanel;
    public MusicController Music;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI StartingText;
    private Animator countDownAnimator;
    private static readonly int CountdownStep = Animator.StringToHash("CountdownStep");
    void Start()
    {
        countDownAnimator = CountdownPanel.GetComponent<Animator>();
        StartingText.text = "GET READY!";
    }

    public IEnumerator StartCounting(Func<SoundEffects> effects)
    {
        yield return new WaitForSeconds(1f); //Giving the game some time to load everything
        CountdownText.gameObject.SetActive(true);
        StartingText.gameObject.SetActive(false);
        effects().PlayEffect(5); //Countdown sound effect

        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            countDownAnimator.SetInteger(CountdownStep, i);
            yield return new WaitForSeconds(1f);
        }

        CountdownText.text = "START!";
        yield return new WaitForSeconds(0.99f);

        CountdownText.gameObject.SetActive(false);
        CountdownPanel.SetActive(false);

        //After the countdown is done, the music will start playing
        Music.PlayMusic();
    }
}
