using Assets.Scripts.Unity.UI.DynamicMessages;
using Assets.Scripts.Unity.UI.Other;
using UnityEngine;

namespace Assets.Scripts.Unity
{
    public class MaterialCounter : MonoBehaviour
    {
        public GameExecuter Executer;
        public DynamicMessage BlockMessage;
        public PopUpMessage BlocksPlus;
        public Warning Warning;
        public int BlocksRemaining = 50;

        /*
         * Executer.Manager.ClearedLayers and Executer.CurrentGame.BlockPlaced
         * can be true for multiple turns.
         * Therefor we need to add extra bools.
         */
        private bool canAddBlocks = false;
        private bool canSubtractBlocks = true;

        void Start()
        {
            BlockMessage.UpdateMessage($"Blocks remaining: {BlocksRemaining}");
            Warning.UniversalConstant = 5;
        }

        void Update()
        {
            //Subtracting blocks if the player cleared some layers
            if (!canSubtractBlocks && Executer.BlocksPlaced == 0) canSubtractBlocks = true;
            else if (Executer.BlocksPlaced > 0 && canSubtractBlocks)
            {
                SubtractBlocks(Executer.BlocksPlaced);
            }

            //Adding blocks if the player cleared some layers
            if (!canAddBlocks && Executer.Manager.ClearedLayers == 0) canAddBlocks = true;
            else if (Executer.Manager.ClearedLayers > 0 && canAddBlocks)
            {
                int extraBlocks = Executer.Manager.ClearedLayers * 15;
                AddBlocks(extraBlocks);
            }

            //If the player runs out of blocks, the game ends
            if (BlocksRemaining <= 0) EndGame();
            Warning.UniversalVariable = BlocksRemaining;
        }

        private void UpdateMessage(int blocks)
        {
            if (blocks >= 0) BlocksPlus.DisplayUpdatedText($"+ {blocks} extra blocks");
            else BlocksPlus.DisplayUpdatedText($"{blocks} blocks");

            BlockMessage.UpdateMessage($"Blocks remaining: {BlocksRemaining}");
        }

        private void AddBlocks(int extraBlocks)
        {
            BlocksRemaining += extraBlocks;
            UpdateMessage(extraBlocks);
            canAddBlocks = false;
        }

        private void SubtractBlocks(int blocksToSubtract)
        {
            BlocksRemaining -= blocksToSubtract;
            UpdateMessage(-blocksToSubtract);
            canSubtractBlocks = false;
        }

        private void EndGame()
        {
            BlockMessage.UpdateMessage("Out of blocks!");
            Executer.CurrentGame.GameOver = true;
        }
    }
}
