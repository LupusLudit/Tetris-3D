using Assets.Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlockHolder"]/*'/>
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

        /// <summary>
        /// Picks a random block.
        /// </summary>
        /// <returns>The picked block.</returns>
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        /// <summary>
        /// Returns new current block (the previously saved <see cref="NextBlock"/>) and generates new <see cref="NextBlock"/>.
        /// </summary>
        /// <returns></returns>
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
