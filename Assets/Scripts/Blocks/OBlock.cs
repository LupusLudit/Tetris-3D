﻿using UnityEngine;


namespace Assets.Scripts.Blocks
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="OBlock"]/*'/>
    public class OBlock : Block
    {
        private Vector3[][][] tiles = new Vector3[][][]
        {
            new Vector3[][]
            {
                new Vector3[]
                {
                    new(1,1,0), new(1,1,1), new(1,0,0), new(1,0,1),
                    new(2,1,0), new(2,1,1), new(2,0,0), new(2,0,1)
                }
            },
            new Vector3[][]
            {
                new Vector3[]
                {
                    new(1,1,0), new(1,1,1), new(1,0,0), new(1,0,1),
                    new(2,1,0), new(2,1,1), new(2,0,0), new(2,0,1)
                }
            },
            new Vector3[][]
            {
                new Vector3[]
                {
                    new(1,1,0), new(1,1,1), new(1,0,0), new(1,0,1),
                    new(2,1,0), new(2,1,1), new(2,0,0), new(2,0,1)
                }
            }
        };


        private readonly Vector3 multiplier;

        public OBlock(Vector3 offsetMultiplier) : base(offsetMultiplier) { }
        protected override Vector3 StartingOffset => new Vector3((int)(3 * OffsetMultiplier.x), (int)(20 * OffsetMultiplier.y), (int)(4 * OffsetMultiplier.z));
        public override int Id => 4;
        protected override Vector3[][][] Tiles => tiles;

    }
}