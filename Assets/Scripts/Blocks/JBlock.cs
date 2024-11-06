﻿using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class JBlock : Block
    {
        private Vector3[][][] tiles = new Vector3[][][]
        {
            new Vector3[][]
            {
                    new Vector3[]{new(1,0,0),new(1,1,0),new(1,1,1),new(1,1,2)},
                    new Vector3[]{new(1,0,1),new(1,0,2),new(1,1,1),new(1,2,1)},
                    new Vector3[]{new(1,1,0),new(1,1,1),new(1,1,2),new(1,2,2)},
                    new Vector3[]{new(1,0,1),new(1,1,1),new(1,2,0),new(1,2,1)}
            },
            new Vector3[][]
            {
                    new Vector3[]{new(0,1,0),new(1,1,0),new(1,1,1),new(1,1,2)},
                    new Vector3[]{new(0,1,1),new(0,1,2),new(1,1,1),new(2,1,1)},
                    new Vector3[]{new(1,1,0),new(1,1,1),new(1,1,2),new(2,1,2)},
                    new Vector3[]{new(0,1,1),new(1,1,1),new(2,1,0),new(2,1,1)}
            },
            new Vector3[][]
            {
                    new Vector3[]{new(0,0,1),new(1,0,1),new(1,1,1),new(1,2,1)},
                    new Vector3[]{new(0,1,1),new(0,2,1),new(1,1,1),new(2,1,1)},
                    new Vector3[]{new(1,0,1),new(1,1,1),new(1,2,1),new(2,2,1)},
                    new Vector3[]{new(0,1,1),new(1,1,1),new(2,0,1),new(2,1,1)}
            }
        };

        private Vector3 multiplier;

        public JBlock(Vector3 multiplier)
        {
            this.multiplier = multiplier;
        }

        protected override Vector3 StartingOffset => new Vector3((int)(3 * OffsetMultiplier.x), 0, (int)(3 * OffsetMultiplier.z));
        public override int Id => 2;
        protected override Vector3[][][] Tiles => tiles;
        protected override Vector3 OffsetMultiplier => multiplier;

    }
}