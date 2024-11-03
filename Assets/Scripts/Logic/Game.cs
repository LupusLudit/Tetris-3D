using Assets.Scripts.Blocks;
using System;
using UnityEngine;

namespace Assets.Scripts
{
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
                    currentBlock.Move(0,1,0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(0,-1,0);
                    }
                }
            }
        }

        public GameGrid Grid { get; }
        public BlockHolder Holder { get; }
        public bool GameOver { get; set; }
        public int Score { get; set; }
        public Block HeldBlock { get; set; }
        public bool CanHold { get; set; }
        public bool BlockPlaced { get; set; }


        public Game(int x, int y, int z)
        {
            Grid = new GameGrid(x,y,z);
            Holder = new BlockHolder();
            CurrentBlock = Holder.GetNextBlock();
            CanHold = true;
            BlockPlaced = false;
        }

        private bool BlockFits()
        {
            foreach (Vector3 v in CurrentBlock.TilePositions())
            {
                if (!Grid.IsEmpty((int)v.x, (int)v.y, (int)v.z)) return false;
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold) return;
            else if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = Holder.GetNextBlock();
            }
            else
            {
                Block temp = HeldBlock;
                HeldBlock = CurrentBlock;
                CurrentBlock = temp;
            }
            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void SwitchToDifAxis(int axis)
        {
            int prevAxis = CurrentBlock.currentState;
            CurrentBlock.SwitchAxis(axis);
            if (!BlockFits()) 
            {
                CurrentBlock.SwitchAxis(prevAxis);
            }
        }

        public void XForward()
        {
            CurrentBlock.Move(1,0,0);
            if (!BlockFits()) CurrentBlock.Move(-1, 0, 0);
        }

        public void XBack()
        {
            CurrentBlock.Move(-1, 0, 0);
            if (!BlockFits()) CurrentBlock.Move(1, 0, 0);
        }

        public void ZForward()
        {
            CurrentBlock.Move(0, 0, 1);
            if (!BlockFits()) CurrentBlock.Move(0, 0, -1);
        }

        public void ZBack()
        {
            CurrentBlock.Move(0, 0, -1);
            if (!BlockFits()) CurrentBlock.Move(0, 0, 1);
        }


        public bool CheckGameOver()
        {
            return !(Grid.IsLayerEmpty(0) && Grid.IsLayerEmpty(1));
        }


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

        public void MoveBlockDown()
        {
            CurrentBlock.Move(0,1,0);

            if (!BlockFits())
            {
                CurrentBlock.Move(0,-1,0);
                PlaceCurrentBlock();

            }
        }

        private int NumOfTilesBelow(Vector3 v)
        {
            int drop = 0;
            while (Grid.IsEmpty((int)v.x, (int)v.y + 1 + drop, (int)v.z))
            {
                drop++;
            }
            return drop;
        }

        public int MaxPossibleDrop()
        {
            int drop = Grid.Y;

            foreach (Vector3 v in CurrentBlock.TilePositions())
            {
                drop = Math.Min(drop, NumOfTilesBelow(v));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(0,MaxPossibleDrop(),0);
            PlaceCurrentBlock();
        }

        public void NextBlock()
        {
            CurrentBlock = Holder.GetNextBlock();
        }

    }
}
