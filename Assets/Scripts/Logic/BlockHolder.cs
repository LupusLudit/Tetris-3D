using Assets.Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class BlockHolder
    {
        private readonly Block[] blocks;
        
        private readonly System.Random random = new System.Random();
        public Block NextBlock { get; private set; }

        public BlockHolder(Vector3 multiplier)
        {
            blocks = new Block[]
            {
                new IBlock(multiplier),
                new JBlock(multiplier),
                new LBlock(multiplier),
                new OBlock(multiplier),
                new SBlock(multiplier),
                new TBlock(multiplier),
                new ZBlock(multiplier)
            };

            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetNewCurrent()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);
            return block;
        }
    }
}
