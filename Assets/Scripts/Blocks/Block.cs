using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Block"]/*'/>
    public abstract class Block
    {
        //Note: Commentary for this class implies for all block overrides as well.

        /// <summary>
        /// The position of the tiles in the 3D space.
        /// To be exact it is the position of the tiles in a "box" surrounding the block.
        /// The size of the "box" is determined by the blocks rotations (it as big as it needs to be to fit all rotations of the block).
        /// </summary>
        /// <value>
        /// 3D Vectors representing the position of the tiles in the 3D space.
        /// </value>
        protected abstract Vector3[][][] Tiles { get; }
        protected abstract Vector3 StartingOffset { get; }
        public abstract int Id { get; }
        public int CurrentState { get; private set; } = 0;
        public int CurrentRotationState { get; private set; } = 0;
        public Vector3 CurrentOffset { get; private set; }

        /// <summary>
        /// The offset multiplier - determines how much should the block really be offset from the original position.
        /// It changes based on board dimensions.
        /// </summary>
        public Vector3 OffsetMultiplier { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="offsetMultiplier">The offset multiplier.</param>
        protected Block(Vector3 offsetMultiplier)
        {
            OffsetMultiplier = offsetMultiplier;
            ResetBlock();
        }

        /// <summary>
        /// IEnumerable of Tiles. Allows to iterate over the tiles using foreach.
        /// </summary>
        /// <returns>
        /// Each Tile one by one.
        /// </returns>
        public IEnumerable<Vector3> TilePositions()
        {
            foreach (Vector3 tile in Tiles[CurrentState][CurrentRotationState])
            {
                yield return tile + CurrentOffset;
            }
        }

        /// <summary>
        /// Rotates the block clockwise.
        /// </summary>
        public void RotateCW()
        {
            CurrentRotationState = (CurrentRotationState + 1) % Tiles[0].Length;
        }

        /// <summary>
        /// Rotates the block counter clockwise.
        /// </summary>
        public void RotateCCW()
        {
            CurrentRotationState = (CurrentRotationState == 0) ? Tiles[0].Length - 1 : CurrentRotationState - 1;
        }

        /// <summary>
        /// Switches the blocks axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public void SwitchAxis(int axis)
        {
            CurrentState = axis;
        }

        /// <summary>
        /// Moves the block by a given distance (determined by the parameters).
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public void Move(int x, int y, int z)
        {
            CurrentOffset += new Vector3(x, y, z);
        }

        /// <summary>
        /// Resets the blocks state(s).
        /// </summary>
        public void ResetBlock()
        {
            CurrentState = 0;
            CurrentRotationState = 0;
            CurrentOffset = StartingOffset;
        }

    }
}
