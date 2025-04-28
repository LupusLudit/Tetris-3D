using Assets.Scripts.Blocks;
using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="ImageDrawer"]/*'/>
    public class ImageDrawer : MonoBehaviour
    {
        public Sprite[] BlockImages;
        public Image NextImage;
        public Image HoldImage;


        /// <summary>
        /// Updates the hold image based on the currently held block.
        /// If no block is held, the image is cleared.
        /// </summary>
        /// <param name="heldBlock">The block currently held by the player. Can be null.</param>
        public void DrawHeldBlock(Block heldBlock)
        {
            HoldImage.sprite = heldBlock != null ? BlockImages[heldBlock.Id - 1] : null;
        }

        /// <summary>
        /// Updates the next block image based on the block stored in the block holder.
        /// </summary>
        /// <param name="holder">The BlockHolder containing the next block to display.</param>
        public void DrawNextBlock(BlockHolder holder)
        {
            NextImage.sprite = BlockImages[holder.NextBlock.Id - 1];
        }

    }
}
