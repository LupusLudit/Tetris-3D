using TMPro;
using UnityEngine;
using Assets.Scripts.Logic;
using Assets.Scripts.Unity.UI.DynamicMessages;

namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="UIManager"]/*'/>
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI LevelUpText;
        public LinesCompleted LinesUI;
        public GameOver GameOverUI;
        public LevelUp LevelUpUI;
        public GameMenu GameMenu;

        /// <summary>
        /// Updates the score text in the UI.
        /// </summary>
        /// <param name="score">The current player's score.</param>
        public void DrawScoreUI(int score)
        {
            ScoreText.text = $"Score: {score}";
        }

        /// <summary>
        /// Displays the lines completed message and the score earned from clearing lines.
        /// </summary>
        /// <param name="score">The score logic instance for calculating earned points.</param>
        /// <param name="level">The current game level.</param>
        /// <param name="numOfCleared">The number of lines cleared at once.</param>
        /// <param name="doubleScore">Whether a double score bonus should be applied.</param>
        public void DrawLinesCompletedUI(Score score, int level, int numOfCleared, bool doubleScore)
        {
            LinesUI.Message.text = score.GetMessage(numOfCleared);
            LinesUI.PlusScore.text = $"+{score.AddLayerScore(level, numOfCleared, doubleScore)}";
            LinesUI.ShowUI();
        }

        /// <summary>
        /// Displays the game over screen.
        /// </summary>
        public void DrawGameOverScreen()
        {
            GameOverUI.ShowEndGameScreen();
        }

        /// <summary>
        /// Updates and shows the level-up UI message.
        /// </summary>
        /// <param name="level">The new player level.</param>
        public void DrawLevelUpUI(int level)
        {
            LevelUpUI.LevelText.text = $"LEVEL: {level}";
            LevelUpText.text = $"LEVEL: {level}";
            LevelUpUI.ShowUI();
        }

        /// <summary>
        /// Pauses the game by showing the pause menu, if not already paused or animating.
        /// </summary>
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