using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Logic;

public class GameModeMenu : MonoBehaviour
{
    public GameObject GameModeMenuUI;
    public GameObject MainMenuUI;
    public Button[] Buttons;
    public TextMeshProUGUI Hint;

    private Animator animator;
    private int hoveredIndex = -1;
    private List<string> buttonHints;

    void Start()
    {
        animator = GameModeMenuUI.GetComponent<Animator>();

        // Load hints from file
        LoadHints();

        for (int i = 0; i < Buttons.Length; i++)
        {
            CheckForButtonHover(i);
        }
    }

    public void StartGame(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void GoBack()
    {
        if (!MainMenuUI.activeSelf)
        {
            StartCoroutine(SlideRightAndDeactivate());
            MainMenuUI.SetActive(true);

        }
    }

    private void LoadHints()
    {
        string filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "gameMode_hints.json");
        HintData data = FileManager.LoadFromFile<HintData>(filePath);

        if (data?.Hints == null)
        {
            return;
        }

        buttonHints = new List<string>(data.Hints);
    }


    private IEnumerator SlideRightAndDeactivate()
    {
        animator.SetTrigger("SlideRight");
        yield return new WaitForSeconds(1f);
        GameModeMenuUI.SetActive(false);
    }

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

    private void OnButtonHover(int index)
    {
        hoveredIndex = index;

        if (index < buttonHints.Count)
        {
            Hint.text = buttonHints[index];
        }
    }

    private void OnButtonExit()
    {
        hoveredIndex = -1;
        Hint.text = "";
    }
}