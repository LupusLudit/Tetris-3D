using Assets.Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts.Logic.Holders
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlockHolder"]/*'/>
    public class BlockHolder : BaseHolder<Block>
    {
        private readonly Vector3 multiplier;

        public BlockHolder(Vector3 multiplier)
        {
            this.multiplier = multiplier;
            InitializeHolder();
        }

        protected override Block[] InitializeItems() =>
            new Block[]
            {
                new IBlock(multiplier),
                new JBlock(multiplier),
                new LBlock(multiplier),
                new OBlock(multiplier),
                new SBlock(multiplier),
                new TBlock(multiplier),
                new ZBlock(multiplier)
            };

        protected override int GetId(Block item) => item.Id;

        public Block NextBlock => NextItem;
        public Block GetNewCurrent() => GetNewNextItem();
    }
}
