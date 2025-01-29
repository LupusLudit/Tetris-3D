using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Warning : MonoBehaviour
{
    public GameObject WarningPanel;
    public GameExecuter Executer;

    private Image warningImage;
    public float blinkSpeed = 4f;

    void Start()
    {
        // Finds the Image component inside the UI Panel
        warningImage = WarningPanel.GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (!Executer.CurrentGame.Grid.IsLayerEmpty(3)) Activate();
        else if (WarningPanel.activeSelf) Deactivate();
    }
    public void Activate()
    {
        WarningPanel.SetActive(true);
        BlinkWarningImage();
    }

    public void Deactivate() =>
        WarningPanel.SetActive(false);

    private void BlinkWarningImage()
    {
        if (warningImage != null)
        {
            float alpha = Mathf.Sin(Time.time * blinkSpeed); // Normalize to 0-1
            Color color = warningImage.color;
            color.a = alpha;
            warningImage.color = color;
        }
    }
}
