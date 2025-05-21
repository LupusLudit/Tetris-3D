using Assets.Scripts.Blocks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlocksTest
{
    private static Vector3 multiplier = new Vector3(1, 1, 1);
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

    [Test]
    public void BlockConstructorTest()
    {
        foreach (var block in blocks)
        {
            Assert.AreEqual(0, block.CurrentState);
            Assert.AreEqual(0, block.CurrentRotationState);
            Assert.AreEqual(multiplier, block.OffsetMultiplier);
        }
    }

    [Test]
    public void BlockResetTest()
    {

        foreach (var block in blocks)
        {
            block.Move(1, -1, 2);
            block.ResetBlock();

            foreach (var tile in block.TilePositions())
            {
                Assert.AreEqual(0, block.CurrentState);
                Assert.AreEqual(0, block.CurrentRotationState);
                // Tile should reset to non-negative starting offset Y after ResetBlock
                Assert.IsTrue(tile.y >= 0);
            }
        }
    }

    [Test]
    public void BlockMovementTest()
    {
        foreach (var block in blocks)
        {
            var original = new List<Vector3>(block.TilePositions());
            block.Move(2, -5, 1);
            var moved = new List<Vector3>(block.TilePositions());

            for (int i = 0; i < original.Count; i++)
            {
                Assert.AreEqual(original[i].x + 2, moved[i].x);
                Assert.AreEqual(original[i].y - 5, moved[i].y);
                Assert.AreEqual(original[i].z + 1, moved[i].z);
            }
        }
    }

    [Test]
    public void BlockRotationTest()
    {
        foreach (var block in blocks)
        {
            var before = new List<Vector3>(block.TilePositions());
            block.RotateCW();
            block.RotateCCW();
            var afterCCW = new List<Vector3>(block.TilePositions());

            // After rotating CW and then CCW, the block should be in its original position
            for (int i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i], afterCCW[i]);
            }
        }
    }

    [Test]
    public void BlockAxisSwitchingTest()
    {
        foreach (var block in blocks)
        {
            int originalState = block.CurrentState;
            block.SwitchAxis((originalState + 1) % 3);
            Assert.AreNotEqual(originalState, block.CurrentState);

            block.SwitchAxis(originalState);
            Assert.AreEqual(originalState, block.CurrentState);
        }
    }
}
