using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.Unity.UI
{
    public class Credits : MonoBehaviour
    {
        public RectTransform CreditsText;
        public float ScrollSpeed = 50f;
        private float initialY;
        private float textHeight;

        void Start()
        {
            initialY = CreditsText.anchoredPosition.y;
            textHeight = CreditsText.rect.height;
        }

        void Update()
        {
            CreditsText.anchoredPosition += Vector2.up * ScrollSpeed * Time.deltaTime;

            if (CreditsText.anchoredPosition.y - textHeight / 2 >= CreditsText.parent.GetComponent<RectTransform>().rect.height / 2)
            {
                CreditsText.anchoredPosition = new Vector2(CreditsText.anchoredPosition.x, initialY);
            }
        }

        public void GoBackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        //music (proper documentation will be added later): https://www.youtube.com/watch?v=0Utp5ogtMxE
    }
}
