﻿using TMPro;
using UnityEngine;
using Assets.Scripts.Logic;
using Assets.Scripts.Unity.UI.DynamicMessages;

namespace Assets.Scripts.Unity.UI
{

    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI LevelUpText;
        public LinesCompleted LinesUI;
        public GameOver GameOverUI;
        public LevelUp LevelUpUI;
        public GameMenu GameMenu;

        public void DrawScoreUI(int score)
        {
            ScoreText.text = $"Score: {score}";
        }

        public void DrawLinesCompletedUI(Score score, int level, int numOfCleared, bool doubleScore)
        {
            LinesUI.Message.text = score.GetMessage(numOfCleared);
            LinesUI.PlusScore.text = $"+{score.AddLayerScore(level, numOfCleared, doubleScore)}";
            LinesUI.ShowUI();
        }

        public void DrawGameOverScreen()
        {
            GameOverUI.ShowEndGameScreen();
        }

        public void DrawLevelUpUI(int level)
        {
            LevelUpUI.LevelText.text = $"LEVEL: {level}";
            LevelUpText.text = $"LEVEL: {level}";
            LevelUpUI.ShowUI();
        }

        public void Pause()
        {
            if (!GameMenu.IsPaused && !GameMenu.IsAnimating)
            {
                GameMenu.IsPaused = true;
                GameMenu.ShowUI();
            }
        }

    }
}