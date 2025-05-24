using System.Collections.Generic;
using Assets.Scripts.Logic;
using NUnit.Framework;
using UnityEngine;

/// <include file='../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameTest"]/*'/>
public class GameTest
{

    /// <summary>
    /// Tests the Game constructor to ensure that essential components (grid, holder, current block)
    /// are properly initialized and that the game starts in a valid state.
    /// </summary>
    [Test]
    public void GameConstructorTest()
    {
        // Test cases for different game configurations
        List<Game> testGames = new List<Game>
        {
            new Game(10, 22, 10),
            new Game(6, 14, 6),
            new Game(5, 20, 15)
        };

        foreach (var game in testGames)
        {
            Assert.IsNotNull(game.Grid);
            Assert.IsNotNull(game.Holder);
            Assert.IsNotNull(game.CurrentBlock);
            Assert.IsTrue(game.CanHold);
            Assert.IsFalse(game.GameOver);
        }
    }

    /// <summary>
    /// Tests the HoldBlock functionality. Verifies the block can be held, swapped,
    /// and that the CanHold bool behaves correctly.
    /// </summary>
    [Test]
    public void HoldBlockTest()
    {
        var game = new Game(10, 20, 10);
        var originalBlock = game.CurrentBlock;

        game.HoldBlock();
        Assert.AreEqual(originalBlock, game.HeldBlock);
        Assert.IsFalse(game.CanHold);

        game.CanHold = true;
        var nextBlock = game.CurrentBlock;
        game.HoldBlock();
        Assert.AreEqual(nextBlock, game.HeldBlock);
        Assert.AreEqual(originalBlock, game.CurrentBlock);
    }

    /// <summary>
    /// Tests the CheckGameOver method to ensure the game correctly identifies
    /// when the top rows are occupied, indicating that the game should end.
    /// </summary>
    [Test]
    public void CheckGameOverTest()
    {
        var game = new Game(10, 22, 10);

        Assert.IsFalse(game.CheckGameOver());

        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                game.Grid[x, 21, z] = 1;
            }
        }

        Assert.IsTrue(game.CheckGameOver());

        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                game.Grid[x, 20, z] = 1;
            }
        }

        Assert.IsTrue(game.CheckGameOver());
    }

    /// <summary>
    /// Tests the MoveBlockDown method to make sure that the block moves down correctly by one unit
    /// and that it is eventually placed after multiple moves.
    /// </summary>
    [Test]
    public void MoveBlockDownTest()
    {
        var game = new Game(10, 20, 10);
        var originalPositions = new List<Vector3>(game.CurrentBlock.TilePositions());

        game.MoveBlockDown();
        var newPositions = new List<Vector3>(game.CurrentBlock.TilePositions());

        for (int i = 0; i < originalPositions.Count; i++)
        {
            Assert.AreEqual(originalPositions[i].x, newPositions[i].x);
            Assert.AreEqual(originalPositions[i].z, newPositions[i].z);
            Assert.AreEqual(originalPositions[i].y - 1, newPositions[i].y);
        }

        for (int i = 0; i < 50 && !game.BlockPlaced; i++)
        {
            game.MoveBlockDown();
        }

        Assert.IsTrue(game.BlockPlaced);
    }

    /// <summary>
    /// Tests the MaxPossibleDrop method to ensure it correctly calculates the
    /// maximum vertical distance the current block can fall.
    /// </summary>
    [Test]
    public void MaxPossibleDropTest()
    {
        var game = new Game(10, 22, 10);

        int maxDrop = game.MaxPossibleDrop();
        int bottomTile = int.MaxValue;
        foreach (var tile in game.CurrentBlock.TilePositions())
        {
            if (tile.y < bottomTile) bottomTile = (int)tile.y;
        }

        /*
         * The block was spawned up in the air.
         * Here, we are checking if the max drop is equal to the distance from the bottom tile to the bottom of the grid
         */
        Assert.AreEqual(bottomTile, maxDrop);
    }

    /// <summary>
    /// Tests the DropBlock method to ensure that the current block drops as it should,
    /// and that it is correctly placed in the game grid.
    /// </summary>
    [Test]
    public void DropBlockTest()
    {
        var game = new Game(10, 20, 10);
        var block = game.CurrentBlock;
        var expectedDrop = game.MaxPossibleDrop();

        game.DropBlock();

        foreach (Vector3 pos in block.TilePositions())
        {
            Assert.AreEqual(block.Id, game.Grid[(int)pos.x, (int)pos.y, (int)pos.z]);
        }

        Assert.IsTrue(game.BlockPlaced);
    }

    /// <summary>
    /// Tests the NextBlock method to ensure the current block is updated to a new one
    /// (the new block should be different from the original one)
    /// and that the CanHold bool was set to true.
    /// </summary>
    [Test]
    public void NextBlockTest()
    {
        var game = new Game(10, 20, 10);
        var currentBlockBefore = game.CurrentBlock;

        game.NextBlock();
        var currentBlockAfter = game.CurrentBlock;

        Assert.AreNotEqual(currentBlockBefore, currentBlockAfter);
        Assert.IsTrue(game.CanHold);
    }

}
