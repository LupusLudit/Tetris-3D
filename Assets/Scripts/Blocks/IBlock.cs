using UnityEngine;

namespace Assets.Scripts.Blocks
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="IBlock"]/*'/>
    public class IBlock : Block
    {
        private readonly Vector3[][][] tiles = new Vector3[][][]
        {
            new Vector3[][]
            {
                new Vector3[]{new(1,2,0),new(1,2,1),new(1,2,2),new(1,2,3)},
                new Vector3[]{new(1,3,2),new(1,2,2),new(1,1,2),new(1,0,2)},
                new Vector3[]{new(1,1,0),new(1,1,1),new(1,1,2),new(1,1,3)},
                new Vector3[]{new(1,3,1),new(1,2,1),new(1,1,1),new(1,0,1)}
            },
            new Vector3[][]
            {
                new Vector3[]{new(1,2,0),new(1,2,1),new(1,2,2),new(1,2,3)},
                new Vector3[]{new(0,2,2),new(1,2,2),new(2,2,2),new(3,2,2)},
                new Vector3[]{new(2,2,0),new(2,2,1),new(2,2,2),new(2,2,3)},
                new Vector3[]{new(0,2,1),new(1,2,1),new(2,2,1),new(3,2,1)}
            },
            new Vector3[][]
            {
                new Vector3[]{new(1,3,1),new(1,2,1),new(1,1,1),new(1,0,1)},
                new Vector3[]{new(0,1,1),new(1,1,1),new(2,1,1),new(3,1,1)},
                new Vector3[]{new(2,3,1),new(2,2,1),new(2,1,1),new(2,0,1)},
                new Vector3[]{new(0,2,1),new(1,2,1),new(2,2,1),new(3,2,1)}
            }
        };

        public IBlock(Vector3 offsetMultiplier) : base(offsetMultiplier) { }
        protected override Vector3 StartingOffset => new Vector3((int)(3 * OffsetMultiplier.x), (int)(19 * OffsetMultiplier.y), (int)(3 * OffsetMultiplier.z));
        public override int Id => 1;
        protected override Vector3[][][] Tiles => tiles;
    }
}
