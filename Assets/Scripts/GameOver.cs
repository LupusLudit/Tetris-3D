using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public MusicController musicController;
    private bool gameOverShown = false;  // Flag to track if the game over UI has been shown

    public void ShowEndGameScreen()
    {
        gameOverUI.SetActive(true);
        musicController.StopMusic();
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        gameOverUI.SetActive(false);
    }
}
