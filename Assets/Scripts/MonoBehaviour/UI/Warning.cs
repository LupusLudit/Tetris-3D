using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Warning : MonoBehaviour
{
    public GameObject WarningPanel;
    public GameExecuter Executer;

    private Image warningImage;
    public float BlinkSpeed = 4f;

    /*
     * Universal constant and variable
     * They can be assigned a value in specific classes (game mods)
     * Example Use:
     * Universal Constant = 10 (10 blocks)
     * Universal Variable = Blocks remaining
     * => If Blocks remaining < 10, show warning
    */
    public int UniversalConstant { get; set; } = 0;
    public int UniversalVariable { get; set; } = 0;
    private bool canDraw = false;

    void Start()
    {
        // Finds the Image component inside the UI Panel
        warningImage = WarningPanel.GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (Executer.IsGameActive())
        {
            if (!canDraw && Executer.BlocksPlaced == 0) canDraw = true;
            else if (NotEnough())
            {
                WarningPanel.SetActive(true);
                Blink();
            }
            else if (BlocksNearTop() && !NotEnough())
            {
                StartCoroutine(ShowOnce());
            }
        }
        else WarningPanel.SetActive(false);
    }
    public IEnumerator ShowOnce()
    {
        WarningPanel.SetActive(true);
        Color color = warningImage.color;
        color.a = 1;
        warningImage.color = color;
        yield return new WaitForSeconds(0.5f);
        WarningPanel.SetActive(false);
    }


    private bool BlocksNearTop()
    {
        return !Executer.CurrentGame.Grid.IsLayerEmpty(3)
            && Executer.BlocksPlaced > 0 && canDraw;
    }

    private bool NotEnough()
    {
        return UniversalVariable < UniversalConstant;
    }

    private void Blink()
    {
        if (warningImage != null)
        {
            float alpha = Mathf.Sin(Time.time * BlinkSpeed); // Normalize to 0-1
            Color color = warningImage.color;
            color.a = alpha;
            warningImage.color = color;
        }
    }
}
