using UnityEngine;

namespace Assets.Scripts.Unity.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        public virtual void ShowUI()
        {
            gameObject.SetActive(true);
        }

        public virtual void HideUI()
        {
            gameObject.SetActive(false);
        }
    }

}
