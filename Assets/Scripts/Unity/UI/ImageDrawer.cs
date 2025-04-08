using Assets.Scripts.Blocks;
using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.UI
{
    public class ImageDrawer : MonoBehaviour
    {
        public Sprite[] BlockImages;
        public Image NextImage;
        public Image HoldImage;

        public void DrawHeldBlock(Block heldBlock)
        {
            HoldImage.sprite = heldBlock != null ? BlockImages[heldBlock.Id - 1] : null;
        }

        public void DrawNextBlock(BlockHolder holder)
        {
            NextImage.sprite = BlockImages[holder.NextBlock.Id - 1];
        }

    }
}
