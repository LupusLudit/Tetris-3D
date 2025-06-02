using Assets.Scripts.Blocks;
using System;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Game"]/*'/>
    public class Game
    {
        private Block currentBlock;
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.ResetBlock();
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.MoveBlock(0,-1,0);
                    if (!BlockFits())
                    {
                        currentBlock.MoveBlock(0,1,0);
                    }
                }
            }
        }

        public GameGrid Grid { get; }
        public BlockHolder Holder { get; }
        public bool GameOver { get; set; }
        public Block HeldBlock { get; set; }
        public bool CanHold { get; set; }
        public bool BlockPlaced { get; set; }

        public Game(int x, int y, int z)
        {
            Grid = new GameGrid(x,y,z);
            Holder = new BlockHolder(CalculateMultiplier(x,y,z));
            CurrentBlock = Holder.GetNewCurrent();
            CanHold = true;
            BlockPlaced = false;
        }

        /// <summary>
        /// Calculates the offset multiplier based on board dimensions.
        /// </summary>
        /// <param name="x">The size of the game board in terms of "x".</param>
        /// <param name="y">The size of the game board in terms of "y".</param>
        /// <param name="z">The size of the game board in terms of "z".</param>
        /// <returns>3D vector representing the offset multiplier.</returns>
        private Vector3 CalculateMultiplier(int x, int y, int z)
        {
            return new Vector3((float) x / 10, (float) y / 21, (float) z / 10);
        }

        /// <summary>
        /// Checks whether the block fits on the board.
        /// </summary>
        /// <returns><c>true</c> it the block fits. Otherwise, <c>false</c>.</returns>
        private bool BlockFits()
        {
            foreach (Vector3 v in CurrentBlock.TilePositions())
            {
                if (!Grid.IsEmpty((int)v.x, (int)v.y, (int)v.z)) return false;
            }
            return true;
        }

        /// <summary>
        /// Holds the current block "in storage", if it was not held already.
        /// </summary>
        public void HoldBlock()
        {
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = Holder.GetNewCurrent();
            }
            else
            {
                Block temp = HeldBlock;
                HeldBlock = CurrentBlock;
                CurrentBlock = temp;
            }
            CanHold = false;
        }

        /// <summary>
        /// Rotates the block to the right if the new orientation fits on the board.
        /// </summary>
        public void TryRotatingBlockToRight()
        {
            CurrentBlock.RotateBlockToRight();

            if (!BlockFits())
            {
                CurrentBlock.RotateBlockToLeft();
            }
        }

        /// <summary>
        /// Rotates the block to the left if the new orientation fits on the board.
        /// </summary>
        public void TryRotatingBlockToLeft()
        {
            CurrentBlock.RotateBlockToLeft();

            if (!BlockFits())
            {
                CurrentBlock.RotateBlockToRight();
            }
        }

        /// <summary>
        /// Switches the block to a "different axis" if the new orientation fits on the board.
        /// </summary>
        /// <param name="axis">The axis "to which we switch".</param>
        public void SwitchToDifAxis(int axis)
        {
            int prevAxis = CurrentBlock.CurrentAxisState;
            CurrentBlock.SwitchAxis(axis);
            if (!BlockFits()) 
            {
                CurrentBlock.SwitchAxis(prevAxis);
            }
        }

        /// <summary>
        /// Moves the block forward in the x direction if the new position fits on the board.
        /// </summary>
        public void XForward()
        {
            CurrentBlock.MoveBlock(1,0,0);
            if (!BlockFits()) CurrentBlock.MoveBlock(-1, 0, 0);
        }

        /// <summary>
        /// Moves the block back in the x direction if the new position fits on the board.
        /// </summary>
        public void XBack()
        {
            CurrentBlock.MoveBlock(-1, 0, 0);
            if (!BlockFits()) CurrentBlock.MoveBlock(1, 0, 0);
        }

        /// <summary>
        /// Moves the block forward in the z direction if the new position fits on the board.
        /// </summary>
        public void ZForward()
        {
            CurrentBlock.MoveBlock(0, 0, 1);
            if (!BlockFits()) CurrentBlock.MoveBlock(0, 0, -1);
        }

        /// <summary>
        /// Moves the block back in the z direction if the new position fits on the board.
        /// </summary>
        public void ZBack()
        {
            CurrentBlock.MoveBlock(0, 0, -1);
            if (!BlockFits()) CurrentBlock.MoveBlock(0, 0, 1);
        }

        /// <summary>
        /// Checks if the top two layers of the grid are empty. If not, the game is over.
        /// </summary>
        /// <returns><c>true</c> it the game should end. Otherwise, <c>false</c>.</returns>
        public bool CheckGameOver()
        {
            return !(Grid.IsLayerEmpty(Grid.Y - 1) && Grid.IsLayerEmpty(Grid.Y - 2));
        }


        /// <summary>
        /// Places the current block onto the bottom of the game board.
        /// </summary>
        private void PlaceCurrentBlock()
        {
            foreach (Vector3 v in CurrentBlock.TilePositions())
            {
                Grid[(int)v.x,(int)v.y,(int)v.z] = CurrentBlock.Id;
            }

            if (CheckGameOver()) GameOver = true;
            else
            { 
                CanHold = true;
                BlockPlaced = true;
            }
        }

        /// <summary>
        /// Moves the block one tile down if the new position fits on the board.
        /// </summary>
        public void MoveBlockDown()
        {
            CurrentBlock.MoveBlock(0,-1,0);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(0,1,0);
                PlaceCurrentBlock();
            }
        }

        /// <summary>
        /// Calculates how many empty tiles are directly below the given position 
        /// until the first occupied tile is encountered (or the bottom of the board).
        /// </summary>
        /// <param name="v">The vector representing the position to check from.</param>
        /// <returns>The number of empty tiles below the given position.</returns>

        private int NumOfTilesBelow(Vector3 v)
        {
            int drop = 0;
            while (Grid.IsEmpty((int)v.x, (int)v.y - 1 - drop, (int)v.z))
            {
                drop++;
            }
            return drop;
        }

        /// <summary>
        /// Determines how far the current block can fall vertically
        /// before colliding with an existing block or the bottom of the grid.
        /// It calculates the minimum distance to the nearest obstacle for all tiles in the block.
        /// </summary>
        /// <returns>The maximum number of tiles the block can drop without overlapping other blocks.</returns>

        public int MaxPossibleDrop()
        {
            int drop = Grid.Y - 1;

            foreach (Vector3 v in CurrentBlock.TilePositions())
            {
                drop = Math.Min(drop, NumOfTilesBelow(v));
            }

            return drop;
        }

        /// <summary>
        /// Instantly drops the current block to its lowest possible position
        /// without collision, then places it on the board.
        /// </summary>
        public void DropBlock()
        {
            CurrentBlock.MoveBlock(0,-MaxPossibleDrop(),0);
            PlaceCurrentBlock();
        }

        /// <summary>
        /// Replaces the current block with the next one from the block holder.
        /// </summary>
        public void NextBlock()
        {
            CurrentBlock = Holder.GetNewCurrent();
        }

    }
}
