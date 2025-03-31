using UnityEngine.SceneManagement;
using UnityEngine;

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
        SceneManager.LoadScene(0);
        GameOverUI.SetActive(false);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverUI.SetActive(false);
    }
}
