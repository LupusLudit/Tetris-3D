using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts.Unity.Audio;

namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameOver"]/*'/>
    public class GameOver : MonoBehaviour
    {
        public GameObject GameOverUI;
        public MusicController MusicController;

        /// <summary>
        /// Displays the Game Over screen and stops the background music.
        /// </summary>
        public void ShowEndGameScreen()
        {
            GameOverUI.SetActive(true);
            MusicController.StopMusic();
        }

        /// <summary>
        /// Loads the Main Menu scene and hides the Game Over UI.
        /// </summary>
        public void ToMenu()
        {
            SceneManager.LoadScene("MainMenu");
            GameOverUI.SetActive(false);
        }

        /// <summary>
        /// Reloads the current scene to restart the game and hides the Game Over UI.
        /// </summary>
        public void PlayAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameOverUI.SetActive(false);
        }
    }
}
