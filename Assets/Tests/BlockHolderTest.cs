using Assets.Scripts.Blocks;
using Assets.Scripts.Logic;
using NUnit.Framework;
using UnityEngine;

public class BlockHolderTest
{
    private Vector3 multiplier = new Vector3(1, 1, 1);

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
