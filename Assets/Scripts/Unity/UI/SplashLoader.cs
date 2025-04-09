using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Unity.UI
{
    public class SplashLoader : MonoBehaviour
    {
        public float delayBeforeLoading = 4f;

        void Start()
        {
            Invoke("LoadNextScene", delayBeforeLoading);
        }

        void LoadNextScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /*
        Note: This whole "LupusLudit Studios" is just a joke, do not take it seriously.
        LupusLudit is just my github username, and I thought it would be funny to use it as a studio name.
        The motto "In stultitia confidimus" translates to "In stupidity we trust", which is a play on the phrase "In God we trust".
        The logo image was generated by the Namecheap website: https://www.namecheap.com/
        */
    }
}