using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public LinesCompleted LinesUI;
        public GameOver GameOverUI;
        public LevelUp LevelUpUI;


        public void DrawScoreUI(int score)
        {
            ScoreText.text = $"Score: {score}";
        }

        public void DrawLinesCompletedUI(Score score, int level, int numOfCleared)
        {
            LinesUI.Message.text = score.GetMessage(numOfCleared);
            LinesUI.PlusScore.text = $"+{score.AddLayerScore(level, numOfCleared)}";
            LinesUI.ShowUI();
        }

        public void DrawGameOverScreen()
        {
            GameOverUI.ShowEndGameScreen();
        }

        public void DrawLevelUpUI(int level)
        {
            LevelUpUI.LevelText.text = $"LEVEL: {level}";
            LevelUpUI.ShowUI();
        }

    }
}
