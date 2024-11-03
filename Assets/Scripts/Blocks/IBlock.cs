﻿using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class IBlock : Block
    {
        private Vector3[][][] tiles = new Vector3[][][]
        {
            new Vector3[][]
            {
                    new Vector3[]{new(1,1,0),new(1,1,1),new(1,1,2),new(1,1,3)},
                    new Vector3[]{new(1,0,2),new(1,1,2),new(1,2,2),new(1,3,2)},
                    new Vector3[]{new(1,2,0),new(1,2,1),new(1,2,2),new(1,2,3)},
                    new Vector3[]{new(1,0,1),new(1,1,1),new(1,2,1),new(1,3,1)}
            },
            new Vector3[][]
            {
                    new Vector3[]{new(1,1,0),new(1,1,1),new(1,1,2),new(1,1,3)},
                    new Vector3[]{new(0,1,2),new(1,1,2),new(2,1,2),new(3,1,2)},
                    new Vector3[]{new(2,1,0),new(2,1,1),new(2,1,2),new(2,1,3)},
                    new Vector3[]{new(0,1,1),new(1,1,1),new(2,1,1),new(3,1,1)}
            },
            new Vector3[][]
            {
                    new Vector3[]{new(1,0,1),new(1,1,1),new(1,2,1),new(1,3,1)},
                    new Vector3[]{new(0,2,1),new(1,2,1),new(2,2,1),new(3,2,1)},
                    new Vector3[]{new(2,0,1),new(2,1,1),new(2,2,1),new(2,3,1)},
                    new Vector3[]{new(0,1,1),new(1,1,1),new(2,1,1),new(3,1,1)}
            }
        };


        protected override Vector3 StartingOffset => new Vector3(3, 0, 3);
        public override int Id => 1;
        protected override Vector3[][][] Tiles => tiles;
    }
}
