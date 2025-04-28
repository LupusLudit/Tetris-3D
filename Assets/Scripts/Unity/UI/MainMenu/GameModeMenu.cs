using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Logic.Managers;

namespace Assets.Scripts.Unity.UI.MainMenu
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameModeMenu"]/*'/>
    public class GameModeMenu : MonoBehaviour
    {
        public GameObject MainMenuUI;
        public Button[] Buttons;
        public TextMeshProUGUI Hint;

        private Animator animator;
        private int hoveredIndex = -1;
        private List<string> buttonHints;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();

            // Load hints from file
            LoadHints();

            for (int i = 0; i < Buttons.Length; i++)
            {
                CheckForButtonHover(i);
            }
        }

        /// <summary>
        /// Starts the game by loading the scene associated with the selected index.
        /// </summary>
        /// <param name="index">Index of the scene to load.</param>
        public void StartGame(int index)
        {
            SceneManager.LoadScene(index);
        }

        /// <summary>
        /// Returns to the main menu UI, playing a slide animation before switching.
        /// </summary>
        public void GoBack()
        {
            if (!MainMenuUI.activeSelf)
            {
                StartCoroutine(SlideRightAndDeactivate());
                MainMenuUI.SetActive(true);

            }
        }

        /// <summary>
        /// Loads hint texts for each game mode button from a JSON file.
        /// </summary>
        private void LoadHints()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "gameMode_hints.json");
            HintData data = FileManager.LoadFromFile<HintData>(filePath, "gameMode_hints.json");

            if (data?.Hints == null)
            {
                return;
            }

            buttonHints = new List<string>(data.Hints);
        }

        /// <summary>
        /// Plays a slide-right animation and then deactivates the GameModeMenu UI.
        /// </summary>
        private IEnumerator SlideRightAndDeactivate()
        {
            animator.SetTrigger("SlideRight");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Adds hover event triggers to a button at the specified index.
        /// </summary>
        /// <param name="index">Button index to set up events for.</param>
        private void CheckForButtonHover(int index)
        {
            EventTrigger trigger = Buttons[index].gameObject.AddComponent<EventTrigger>();

            // PointerEnter event
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnButtonHover(index); });
            trigger.triggers.Add(entryEnter);

            // PointerExit event
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnButtonExit(); });
            trigger.triggers.Add(entryExit);
        }

        /// <summary>
        /// Called when the pointer enters a button, updating the hint text.
        /// </summary>
        /// <param name="index">Index of the button being hovered over.</param>
        private void OnButtonHover(int index)
        {
            hoveredIndex = index;

            if (index < buttonHints.Count)
            {
                Hint.text = buttonHints[index];
            }
        }

        /// <summary>
        /// Called when the pointer exits a button, clearing the hint text.
        /// </summary>
        private void OnButtonExit()
        {
            hoveredIndex = -1;
            Hint.text = "";
        }
    }
}