using Assets.Scripts.Unity.UI.DynamicMessages;
using Assets.Scripts.Unity.UI.Other;
using UnityEngine;

namespace Assets.Scripts.Unity
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="MaterialCounter"]/*'/>
    public class MaterialCounter : MonoBehaviour
    {
        public GameExecuter Executer;
        public DynamicMessage BlockMessage;
        public PopUpMessage BlocksPlus;
        public Warning Warning;
        public int BlocksRemaining = 50;

        /*
         * Executer.Manager.ClearedLayers and Executer.CurrentGame.BlockPlaced
         * can be true for multiple turns, so we need to add extra Booleans.
         */
        private bool canAddBlocks = false;
        private bool canSubtractBlocks = true;

        void Start()
        {
            BlockMessage.UpdateMessage($"Blocks remaining: {BlocksRemaining}");
            Warning.UniversalConstant = 5;
        }

        /// <summary>
        /// Updates the block counter every frame based on player actions.
        /// Handles subtraction on block placement, addition on layer clears, 
        /// and triggers game over if no blocks are left.
        /// </summary>
        void Update()
        {
            // Subtracting blocks if the player cleared some layers
            if (!canSubtractBlocks && Executer.BlocksPlaced == 0) canSubtractBlocks = true;
            else if (Executer.BlocksPlaced > 0 && canSubtractBlocks)
            {
                SubtractBlocks(Executer.BlocksPlaced);
            }

            // Adding blocks if the player cleared some layers
            if (!canAddBlocks && Executer.Manager.ClearedLayers == 0) canAddBlocks = true;
            else if (Executer.Manager.ClearedLayers > 0 && canAddBlocks)
            {
                int extraBlocks = Executer.Manager.ClearedLayers * 15;
                AddBlocks(extraBlocks);
            }

            // If the player runs out of blocks, the game ends
            if (BlocksRemaining <= 0) EndGame();
            Warning.UniversalVariable = BlocksRemaining;
        }

        /// <summary>
        /// Updates both the pop-up and dynamic messages to reflect the current block count change.
        /// </summary>
        /// <param name="blocks">The number of blocks added (positive) or subtracted (negative).</param>
        private void UpdateMessage(int blocks)
        {
            if (blocks >= 0) BlocksPlus.DisplayUpdatedText($"+ {blocks} extra blocks");
            else BlocksPlus.DisplayUpdatedText($"{blocks} blocks");

            BlockMessage.UpdateMessage($"Blocks remaining: {BlocksRemaining}");
        }

        /// <summary>
        /// Adds a specified number of extra blocks to the player's total.
        /// </summary>
        /// <param name="extraBlocks">Number of blocks to add.</param>
        private void AddBlocks(int extraBlocks)
        {
            BlocksRemaining += extraBlocks;
            UpdateMessage(extraBlocks);
            canAddBlocks = false;
        }

        /// <summary>
        /// Subtracts a specified number of blocks when the player places blocks.
        /// </summary>
        /// <param name="blocksToSubtract">Number of blocks to subtract.</param>
        private void SubtractBlocks(int blocksToSubtract)
        {
            BlocksRemaining -= blocksToSubtract;
            UpdateMessage(-blocksToSubtract);
            canSubtractBlocks = false;
        }

        /// <summary>
        /// Ends the material game when the player runs out of blocks and displays a game over message.
        /// </summary>
        private void EndGame()
        {
            BlockMessage.UpdateMessage("Out of blocks!");
            Executer.CurrentGame.GameOver = true;
        }
    }
}
