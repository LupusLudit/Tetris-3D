using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

public class LevelUp : MonoBehaviour
{

    public GameObject levelUI;
    public TextMeshProUGUI LevelText;

    public void ShowUI()
    {
        levelUI.SetActive(true);
    }

    public void HideUI()
    {
        levelUI.SetActive(false);
    }
}
