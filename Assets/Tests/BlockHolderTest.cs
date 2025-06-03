using Assets.Scripts.Blocks;
using Assets.Scripts.Logic.Holders;
using NUnit.Framework;
using UnityEngine;

/// <include file='../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlockHolderTest"]/*'/>
public class BlockHolderTest
{
    private Vector3 multiplier = new Vector3(1, 1, 1);

    /// <summary>
    /// Tests that the BlockHolder constructor initializes correctly.
    /// </summary>
    [Test]
    public void BlockHolderConstructorTest()
    {
        for (int i = 0; i < 10; i++) 
        {
            BlockHolder holder = new BlockHolder(multiplier);
            Assert.IsNotNull(holder);
            Assert.IsNotNull(holder.NextBlock);
        }
    }

    /// <summary>
    /// Tests that GetNewCurrent() returns a new current block and updates the NextBlock.
    /// It makes sure that the newly set NextBlock is different from the previous one.
    /// </summary>
    [Test]
    public void GetNewCurrentTest()
    {
        for (int i = 0; i < 10; i++)
        {
            BlockHolder holder = new BlockHolder(multiplier);
            Block prevNext = holder.NextBlock;
            for (int j = 0; j < 50; j++)
            {
                Block current = holder.GetNewCurrent();
                Assert.IsNotNull(current);
                Assert.IsNotNull(holder.NextBlock);

                Assert.AreNotEqual(prevNext.Id, holder.NextBlock.Id);

                prevNext = holder.NextBlock;
            }
        }
    }
}
