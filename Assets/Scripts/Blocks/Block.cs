using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public abstract class Block
    {
        protected abstract Vector3[][][] Tiles { get; }
        protected abstract Vector3 StartingOffset { get; }
        public abstract int Id { get; }
        public int CurrentState { get; private set; } = 0;
        public int CurrentRotationState { get; private set; } = 0;
        public Vector3 CurrentOffset { get; private set; }
        private readonly Vector3 offsetMultiplier;

        protected Block(Vector3 offsetMultiplier)
        {
            this.offsetMultiplier = offsetMultiplier;
            ResetBlock();
        }

        public IEnumerable<Vector3> TilePositions()
        {
            foreach (Vector3 tile in Tiles[CurrentState][CurrentRotationState])
            {
                yield return tile + CurrentOffset;
            }
        }

        public void RotateCW()
        {
            CurrentRotationState = (CurrentRotationState + 1) % Tiles[0].Length;
        }

        public void RotateCCW()
        {
            CurrentRotationState = (CurrentRotationState == 0) ? Tiles[0].Length - 1 : CurrentRotationState - 1;
        }

        public void SwitchAxis(int axis)
        {
            CurrentState = axis;
        }

        public void Move(int x, int y, int z)
        {
            CurrentOffset += new Vector3(x, y, z);
        }

        public void ResetBlock()
        {
            CurrentState = 0;
            CurrentRotationState = 0;
            CurrentOffset = StartingOffset;
        }

        protected Vector3 OffsetMultiplier => offsetMultiplier;
    }
}
