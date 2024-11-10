using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Blocks
{
    public abstract class Block
    {
        protected abstract Vector3[][][] Tiles { get; }
        protected abstract Vector3 StartingOffset { get; } //Generic offset used for 10x20(22)x10 board (regular mode), for different sizes of the board we use the multiplier
        protected abstract Vector3 OffsetMultiplier { get; } //Its being used to calculate the Starting offset based on the board size
        public abstract int Id { get; }
        public int CurrentState; //0-2
        public int CurrentRotationState; //0-3
        public Vector3 CurrentOffset;

        public Block()
        {
            CurrentOffset = new Vector3(StartingOffset.x, StartingOffset.y, StartingOffset.z);
        }

        public IEnumerable<Vector3> TilePositions()
        {
            foreach (Vector3 v in Tiles[CurrentState][CurrentRotationState])
            {
                yield return new Vector3(v.x + CurrentOffset.x, v.y + CurrentOffset.y, v.z + CurrentOffset.z);
            }
        }

        public void RotateCW()
        {
            CurrentRotationState = (CurrentRotationState + 1) % Tiles[0].Length; ;
        }

        public void RotateCCW()
        {
            if (CurrentRotationState == 0)
            {
                CurrentRotationState = Tiles[0].Length - 1;
            }
            else
            {
                CurrentRotationState--;
            }
        }
        public void SwitchAxis(int axis)
        {
            CurrentState = axis;
        }

        public void Move(int x, int y, int z) 
        {
            CurrentOffset.x += x;
            CurrentOffset.y += y;
            CurrentOffset.z += z;
        }

        public void ResetBlock()
        {
            CurrentState = 0;
            CurrentRotationState = 0;
            CurrentOffset = StartingOffset;
        }

    }
}
