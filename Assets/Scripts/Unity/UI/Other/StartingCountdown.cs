using Assets.Scripts.Unity.Audio;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Unity.UI.Other
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="StartingCountdown"]/*'/>
    public class StartingCountdown : MonoBehaviour
    {
        public MusicController Music;
        public TextMeshProUGUI CountdownText;
        public TextMeshProUGUI StartingText;
        private Animator countDownAnimator;
        private static readonly int CountdownStep = Animator.StringToHash("CountdownStep");
        void Start()
        {
            countDownAnimator = gameObject.GetComponent<Animator>();
            StartingText.text = "GET READY!";
        }

        /// <summary>
        /// Coroutine that runs the countdown sequence, updates the UI and triggers sound effects.
        /// </summary>
        /// <param name="effects">A function returning a SoundEffects instance used to play countdown sounds.</param>
        /// <returns>An IEnumerator to be used with StartCoroutine.</returns>
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
            gameObject.SetActive(false);

            //After the countdown is done, the music will start playing
            Music.PlayMusic();
        }
    }
}
