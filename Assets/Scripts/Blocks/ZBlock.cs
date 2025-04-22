using UnityEngine;

namespace Assets.Scripts.Blocks
{
    /// <summary>
    /// TODO: add a link here
    /// </summary>
    public class ZBlock : Block
    {
        private Vector3[][][] tiles = new Vector3[][][]
        {
            new Vector3[][]
            {
                new Vector3[]{new(1,2,0),new(1,2,1),new(1,1,1),new(1,1,2)},
                new Vector3[]{new(1,2,2),new(1,1,1),new(1,1,2),new(1,0,1)},
                new Vector3[]{new(1,1,0),new(1,1,1),new(1,0,1),new(1,0,2)},
                new Vector3[]{new(1,2,1),new(1,1,0),new(1,1,1),new(1,0,0)}
            },
            new Vector3[][]
            {
                new Vector3[]{new(0,1,0),new(0,1,1),new(1,1,1),new(1,1,2)},
                new Vector3[]{new(0,1,2),new(1,1,1),new(1,1,2),new(2,1,1)},
                new Vector3[]{new(1,1,0),new(1,1,1),new(2,1,1),new(2,1,2)},
                new Vector3[]{new(0,1,1),new(1,1,0),new(1,1,1),new(2,1,0)}
            },
            new Vector3[][]
            {
                new Vector3[]{new(0,2,1),new(0,1,1),new(1,1,1),new(1,0,1)},
                new Vector3[]{new(0,0,1),new(1,1,1),new(1,0,1),new(2,1,1)},
                new Vector3[]{new(1,2,1),new(1,1,1),new(2,1,1),new(2,0,1)},
                new Vector3[]{new(0,1,1),new(1,2,1),new(1,1,1),new(2,2,1)}
            }
        };

        private readonly Vector3 multiplier;

        public ZBlock(Vector3 offsetMultiplier) : base(offsetMultiplier) { }
        protected override Vector3 StartingOffset => new Vector3((int)(3 * OffsetMultiplier.x), (int)(19 * OffsetMultiplier.y), (int)(3 * OffsetMultiplier.z));
        public override int Id => 7;
        protected override Vector3[][][] Tiles => tiles;
    }
}
