using Assets.Scripts.Blocks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <include file='../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlocksTest"]/*'/>
public class BlocksTest
{
    private static Vector3 multiplier = new Vector3(1, 1, 1);

    /// <summary>
    /// A list of all Tetris block types used in the game.
    /// </summary>
    List<Block> blocks = new List<Block>()
    {
        new IBlock(multiplier),
        new JBlock(multiplier),
        new LBlock(multiplier),
        new OBlock(multiplier),
        new SBlock(multiplier),
        new TBlock(multiplier),
        new ZBlock(multiplier)
    };

    /// <summary>
    /// Test that each block is initialized with the correct default values.
    /// </summary>
    [Test]
    public void BlockConstructorTest()
    {
        foreach (var block in blocks)
        {
            Assert.AreEqual(0, block.CurrentAxisState);
            Assert.AreEqual(0, block.CurrentRotationState);
            Assert.AreEqual(multiplier, block.OffsetMultiplier);
        }
    }

    /// <summary>
    /// Test that ResetBlock resets block position and states correctly.
    /// </summary>
    [Test]
    public void BlockResetTest()
    {

        foreach (var block in blocks)
        {
            block.Move(1, -1, 2);
            block.ResetBlock();

            foreach (var tile in block.TilePositions())
            {
                Assert.AreEqual(0, block.CurrentAxisState);
                Assert.AreEqual(0, block.CurrentRotationState);
                // Tile should reset to non-negative starting offset Y after ResetBlock
                Assert.IsTrue(tile.y >= 0);
            }
        }
    }

    /// <summary>
    /// Tests that the Move() method correctly changes all tile positions
    /// after moving the block by a randomly generated offset (xOffset, yOffset, zOffset).
    /// </summary>
    [Test]
    public void BlockMovementTest()
    {
        System.Random random = new System.Random();
        foreach (var block in blocks)
        {
            int xOffset = random.Next(-5, 11);
            int yOffset = random.Next(-5, 11);
            int zOffset = random.Next(-5, 11);

            var original = new List<Vector3>(block.TilePositions());
            block.Move(xOffset, yOffset, zOffset);
            var moved = new List<Vector3>(block.TilePositions());

            for (int i = 0; i < original.Count; i++)
            {
                Assert.AreEqual(original[i].x + xOffset, moved[i].x);
                Assert.AreEqual(original[i].y + yOffset, moved[i].y);
                Assert.AreEqual(original[i].z + zOffset, moved[i].z);
            }
        }
    }

    /// <summary>
    /// Tests if rotating a block clockwise and then counterclockwise results in the original state.
    /// </summary>
    [Test]
    public void BlockRotationTest()
    {
        foreach (var block in blocks)
        {
            var before = new List<Vector3>(block.TilePositions());
            block.RotateRight();
            block.RotateLeft();
            var afterCCW = new List<Vector3>(block.TilePositions());

            // After rotating CW and then CCW, the block should be in its original position
            for (int i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i], afterCCW[i]);
            }
        }
    }

    /// <summary>
    /// Tests if the SwitchAxis() method correctly changes the block's axis state.
    /// </summary>
    [Test]
    public void BlockAxisSwitchingTest()
    {
        foreach (var block in blocks)
        {
            int originalState = block.CurrentAxisState;
            block.SwitchAxis((originalState + 1) % 3);
            Assert.AreNotEqual(originalState, block.CurrentAxisState);

            block.SwitchAxis(originalState);
            Assert.AreEqual(originalState, block.CurrentAxisState);
        }
    }
}
