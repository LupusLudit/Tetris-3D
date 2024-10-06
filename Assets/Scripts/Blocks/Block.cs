using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
namespace Assets.Scripts.Blocks
{
    public abstract class Block
    {
        protected abstract Vector3[][][] Tiles { get; }
        protected abstract Vector3 StartingOffset { get; }

        public abstract int Id { get; }
        public int currentState; //0-2
        public int currentRotationState; //0-3
        public Vector3 currentOffset;

        private bool tilesDefault = true;

        public Block()
        {
            currentOffset = new Vector3(StartingOffset.x, StartingOffset.y, StartingOffset.z);
        }

        public IEnumerable<Vector3> TilePositions()
        {
            foreach (Vector3 v in Tiles[currentState][currentRotationState])
            {
                yield return new Vector3(v.x + currentOffset.x, v.y + currentOffset.y, v.z + currentOffset.z);
            }
        }

        public void RotateCW()
        {
            currentRotationState = (currentRotationState + 1) % Tiles[0].Length; ;
        }

        public void RotateCCW()
        {
            if (currentRotationState == 0)
            {
                currentRotationState = Tiles[0].Length - 1;
            }
            else
            {
                currentRotationState--;
            }
        }
        public void SwitchAxis(int axis)
        {
            currentState = axis;
        }

        public void Move(int x, int y, int z) 
        {
            currentOffset.x += x;
            currentOffset.y += y;
            currentOffset.z += z;
        }

        public void ResetBlock()
        {
            currentState = 0;
            currentRotationState = 0;

            currentOffset.x = StartingOffset.x;
            currentOffset.y = StartingOffset.y;
            currentOffset.z = StartingOffset.z;
        }

    }
}
