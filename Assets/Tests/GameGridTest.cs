using Assets.Scripts.Logic;
using NUnit.Framework;
using System.Collections.Generic;

/// <include file='../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameGridTest"]/*'/>
public class GameGridTest
{
    private System.Random random = new System.Random();

    /// <summary>
    /// Generates a list of GameGrid instances with random dimensions for testing.
    /// </summary>
    /// <param name="numOfGrids">Number of grids to generate.</param>
    /// <returns>A list of randomly sized GameGrid instances.</returns>
    private List<GameGrid> GenerateRandomGrids(int numOfGrids)
    {
        List<GameGrid> grids = new List<GameGrid>();
        for (int i = 0; i < numOfGrids; i++)
        {
            grids.Add(new GameGrid(random.Next(5, 30), random.Next(5, 30), random.Next(5, 30)));
        }
        return grids;
    }

    /// <summary>
    /// Tests whether the IsInBounds method correctly identifies in-bound and out-of-bound coordinates.
    /// </summary>
    [Test]
    public void IsInBoundsTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            Assert.IsTrue(grid.IsInBounds(0, 0, 0));
            Assert.IsTrue(grid.IsInBounds(grid.X - 1, grid.Y - 1, grid.Z - 1));
            Assert.IsFalse(grid.IsInBounds(-1, -1, -1));
            Assert.IsFalse(grid.IsInBounds(grid.X+1, grid.Y+1, grid.Z+1));
            Assert.IsFalse(grid.IsInBounds(grid.X + 3, grid.Y, grid.Z));
        }
    }

    /// <summary>
    /// Tests whether the IsEmpty method correctly identifies if a cell is unoccupied or out of bounds.
    /// </summary>
    [Test]
    public void IsEmptyTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            Assert.IsTrue(grid.IsEmpty(0, 0, 0));
            Assert.IsTrue(grid.IsEmpty(grid.X - 1, grid.Y - 1, grid.Z - 1));
            Assert.IsFalse(grid.IsEmpty(-1, -1, -1));
            Assert.IsFalse(grid.IsEmpty(grid.X + 3, grid.Y, grid.Z));

            // Set a cell to a non-zero value
            int x = random.Next(0, grid.X);
            int y = random.Next(0, grid.Y);
            int z = random.Next(0, grid.Z);
            grid[x,y,z] = 1;
            Assert.IsFalse(grid.IsEmpty(x, y, z));
        }
    }

    /// <summary>
    /// Tests whether IsLayerFull correctly identifies fully occupied layers and layers with cleared cells.
    /// </summary>
    [Test]
    public void IsLayerFullTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            int y = random.Next(0, grid.Y);
            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, y, z] = 1;
                }
            }
            Assert.IsTrue(grid.IsLayerFull(y));

            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, y, z] = 0;
                }
            }
            Assert.IsFalse(grid.IsLayerFull(y));
        }
    }

    /// <summary>
    /// Tests whether IsLayerEmpty correctly identifies completely empty and non-empty layers.
    /// </summary>
    [Test]
    public void IsLayerEmptyTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            int y = random.Next(0, grid.Y);
            Assert.IsTrue(grid.IsLayerEmpty(y));

            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, y, z] = 1;
                }
            }
            Assert.IsFalse(grid.IsLayerEmpty(y));

            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, y, z] = 0;
                }
            }
            Assert.IsTrue(grid.IsLayerEmpty(y));

            grid[random.Next(0, grid.X), y, random.Next(0, grid.Z)] = 1;
            Assert.IsFalse(grid.IsLayerEmpty(y));
        }
    }

    /*
     * The ClearLayerTest, ClearColumnTest and MoveLayerDownTest assume that IsLayerEmpty and IsLayerFull methods are functioning correctly.
     * Therefore IsLayerEmptyTest and IsLayerFullTest have to be executed before these tests to ensure correct behavior.
     */

    /// <summary>
    /// Tests whether ClearLayer empties all cells in a randomly picked y-layer.
    /// </summary>
    [Test]
    public void ClearLayerTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            int y = random.Next(0, grid.Y);
            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, y, z] = 1;
                }
            }
            Assert.IsFalse(grid.IsLayerEmpty(y));

            grid.ClearLayer(y);
            Assert.IsTrue(grid.IsLayerEmpty(y));

            grid[random.Next(0, grid.X), y, random.Next(0, grid.Z)] = 1;
            grid.ClearLayer(y);
            Assert.IsTrue(grid.IsLayerEmpty(y));
        }
    }

    /// <summary>
    /// Tests whether ClearColumn removes all values from
    /// a randomly picked vertical column at (x, z).
    /// </summary>
    [Test]
    public void ClearColumnTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            int x = random.Next(0, grid.X);
            int z = random.Next(0, grid.Z);

            for (int y = 0; y < grid.Y; y++)
            {
                grid[x, y, z] = 1;
            }

            for (int y = 0; y < grid.Y; y++)
            {
                Assert.IsFalse(grid.IsLayerEmpty(y));
            }

            grid.ClearColumn(x, z);
            for (int y = 0; y < grid.Y; y++)
            {
                Assert.AreEqual(0, grid[x, y, z]);
                Assert.IsTrue(grid.IsLayerEmpty(y));
            }

            int newX = (x + 1) % grid.X;
            int newZ = (z + 1) % grid.Z;
            int randomY = random.Next(0, grid.Y);
            grid[newX, randomY, newZ] = 1;

            Assert.IsFalse(grid.IsLayerEmpty(randomY));

            grid.ClearColumn(x, z);
            Assert.IsFalse(grid.IsLayerEmpty(randomY));
        }
    }

    /// <summary>
    /// Tests whether MoveLayerDown shifts a layer downward by a randomly generated number of rows,
    /// and clears the original source layer.
    /// </summary>
    [Test]
    public void MoveLayerDownTest()
    {
        List<GameGrid> grids = GenerateRandomGrids(20);
        foreach (GameGrid grid in grids)
        {
            for (int x = 0; x < grid.X; x++)
            {
                for (int z = 0; z < grid.Z; z++)
                {
                    grid[x, grid.Y-1, z] = 1;
                }
            }
            Assert.IsFalse(grid.IsLayerEmpty(grid.Y - 1));

            int rowsDown = random.Next(1, grid.Y);
            grid.MoveLayerDown(grid.Y - 1, rowsDown);

            Assert.IsTrue(grid.IsLayerEmpty(grid.Y - 1));
            Assert.IsTrue(grid.IsLayerFull(grid.Y -1 - rowsDown));
        }
    }
}
