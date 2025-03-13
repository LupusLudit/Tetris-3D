using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class Bomb : PowerUp
    {

        private HashSet<GameObject> tiles;
        public override int Id => 4;

        public override string Title => "Bomb";

        public override string Description => "Boom! Blocks around you have been destroyed";

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

        public Bomb(GameExecuter executer) : base(executer)
        {
            tiles = executer.Manager.PlacedBlocks;
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
                TilePoolManager.Instance.ReturnTile(tile);
            }
        }

    }
}
