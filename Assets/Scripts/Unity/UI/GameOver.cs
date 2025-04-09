using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts.Unity.Audio;

namespace Assets.Scripts.Unity.UI
{
    public class GameOver : MonoBehaviour
    {
        public GameObject GameOverUI;
        public MusicController MusicController;

        public void ShowEndGameScreen()
        {
            GameOverUI.SetActive(true);
            MusicController.StopMusic();
        }

        public void ToMenu()
        {
            SceneManager.LoadScene("MainMenu");
            GameOverUI.SetActive(false);
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameOverUI.SetActive(false);
        }
    }
}
