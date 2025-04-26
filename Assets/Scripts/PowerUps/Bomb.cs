using Assets.Scripts.Unity;
using Assets.Scripts.Unity.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Bomb"]/*'/>
    public class Bomb : PowerUp
    {

        private HashSet<GameObject> tiles;
        public override int Id => 4;

        public override string Title => "Bomb";

        public override string Description => "Boom! Blocks around you have been destroyed";


        /// <summary>
        /// Represents the positions around the bomb that will be affected by the explosion.
        /// </summary>
        private Vector3[] blastRadiusPositions;

        public Bomb(GameExecuter executer) : base(executer)
        {
            tiles = executer.Manager.PlacedBlocks;
            blastRadiusPositions = new Vector3[]
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
        }

        /// <summary>
        /// Destroys all tiles located within the defined blast radius.
        /// Iterates through all current tiles and checks if any tile's position matches a position 
        /// within the <see cref="blastRadiusPositions"/>.
        /// </summary>

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
                TilePoolManager.Instance.ReturnTile(tile);
                tiles.Remove(tile);
            }
        }

    }
}
