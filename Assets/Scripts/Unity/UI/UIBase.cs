using UnityEngine;

namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="UIBase"]/*'/>
    public abstract class UIBase : MonoBehaviour
    {
        /// <summary>
        /// Displays the UI element by activating the associated GameObject.
        /// </summary>
        public virtual void ShowUI()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the UI element by deactivating the associated GameObject.
        /// </summary>
        public virtual void HideUI()
        {
            gameObject.SetActive(false);
        }
    }

}
