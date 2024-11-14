using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class Bomb : PowerUp
    {

        private HashSet<GameObject> tiles;
        private Vector3[] blastRadiusPositions =
        {
            new Vector3(-1, -1, -1), new Vector3(-1, -1, 0), new Vector3(-1, -1, 1),
            new Vector3(-1, 0, -1), new Vector3(-1, 0, 0), new Vector3(-1, 0, 1),
            new Vector3(-1, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 1, 1),

            new Vector3(0, -1, -1), new Vector3(0, -1, 0), new Vector3(0, -1, 1),
            new Vector3(0, 0, -1), new Vector3(0, 0, 1),
            new Vector3(0, 1, -1), new Vector3(0, 1, 0), new Vector3(0, 1, 1),

            new Vector3(1, -1, -1), new Vector3(1, -1, 0), new Vector3(1, -1, 1),
            new Vector3(1, 0, -1), new Vector3(1, 0, 0), new Vector3(1, 0, 1),
            new Vector3(1, 1, -1), new Vector3(1, 1, 0), new Vector3(1, 1, 1)
        };

        public Bomb(Vector3 position, GameExecuter executer) : base(position, executer)
        {
            tiles = executer.PlacedBlocks;
        }

        public override void Use()
        {
            HashSet<GameObject> tilesToRemove = new HashSet<GameObject>();
            foreach (var tile in tiles)
            {
                foreach (var offset in blastRadiusPositions)
                {
                    Vector3 targetPosition = Position + offset;
                    if (tile.transform.position == targetPosition)
                    {
                        tilesToRemove.Add(tile);
                        break;
                    }
                }
            }
            foreach (var tile in tilesToRemove)
            {
                tiles.Remove(tile);
                Executer.RemoveTile(tile);
            }
        }

    }
}
