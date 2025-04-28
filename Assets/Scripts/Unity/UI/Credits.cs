using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Credits"]/*'/>

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

        /// <summary>
        /// Updates the position of the credits text to scroll upwards.
        /// Resets the position once the text has fully passed through the view.
        /// </summary>
        void Update()
        {
            CreditsText.anchoredPosition += Vector2.up * ScrollSpeed * Time.deltaTime;

            if (CreditsText.anchoredPosition.y - textHeight / 2 >= CreditsText.parent.GetComponent<RectTransform>().rect.height / 2)
            {
                CreditsText.anchoredPosition = new Vector2(CreditsText.anchoredPosition.x, initialY);
            }
        }

        /// <summary>
        /// Loads the Main Menu scene when called.
        /// </summary>
        public void GoBackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
