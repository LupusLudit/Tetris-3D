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

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        GameOverUI.SetActive(false);
    }
}
