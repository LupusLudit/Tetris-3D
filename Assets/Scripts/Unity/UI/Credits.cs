using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed = 50f;
    private float initialY;
    private float textHeight;

    void Start()
    {
        initialY = creditsText.anchoredPosition.y;
        textHeight = creditsText.rect.height;
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y - textHeight / 2 >= creditsText.parent.GetComponent<RectTransform>().rect.height/2)
        {
            creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, initialY);
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //music (proper documentation will be added later): https://www.youtube.com/watch?v=0Utp5ogtMxE
}
