using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unity.ObjectPooling
{
    public class TilePoolManager : MonoBehaviour
    {
        public TilePoolConfig[] tileConfigs;
        private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

        public static TilePoolManager Instance { get; private set; }

        /// <summary>
        /// Sets up the singleton instance and initializes pools for each configured prefab.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                foreach (TilePoolConfig config in tileConfigs)
                {
                    Queue<GameObject> pool = new Queue<GameObject>();
                    for (int i = 0; i < config.poolSize; i++)
                    {
                        AddTileToPool(config.prefab, pool);
                    }
                    pools.Add(config.prefab, pool);
                }
            }
        }

        /// <summary>
        /// Retrieves a tile GameObject from the pool corresponding to the given prefab.
        /// If the pool is empty, a new tile is instantiated and added to the pool before retrieval.
        /// </summary>
        /// <param name="prefab">The prefab representing the type of tile to retrieve.</param>
        /// <returns> Tile GameObject ready for use.</returns>
        public GameObject GetTile(GameObject prefab)
        {
            var pool = pools[prefab];

            if (pool.Count == 0)
            {
                AddTileToPool(prefab, pool);
            }

            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns a tile GameObject back to its pool (queue) for reuse.
        /// Deactivates the tile and resets its parent transform to the TilePoolManager.
        /// The tile does not get destroyed, rather it is set inactive, waiting for the request to be reused again.
        /// </summary>
        /// <param name="tile">The tile GameObject to return to the pool.</param>
        public void ReturnTile(GameObject tile)
        {
            tile.isStatic = false;
            var pooledTile = tile.GetComponent<PooledTile>();

            tile.SetActive(false);
            tile.transform.SetParent(this.transform);
            pools[pooledTile.prefabReference].Enqueue(tile);
        }

        /// <summary>
        /// Instantiates a new tile GameObject from the prefab,
        /// deactivates it, and adds it to the specified pool (queue).
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="pool">The pool queue to add the new tile to.</param>
        private void AddTileToPool(GameObject prefab, Queue<GameObject> pool)
        {
            GameObject tile = Instantiate(prefab);
            var pooledTile = tile.AddComponent<PooledTile>();
            pooledTile.prefabReference = prefab;

            tile.SetActive(false);
            tile.transform.SetParent(this.transform);
            pool.Enqueue(tile);
        }

    }

    [System.Serializable]
    public class TilePoolConfig
    {
        public GameObject prefab;
        public int poolSize = 100;
    }

    public class PooledTile : MonoBehaviour
    {
        public GameObject prefabReference;
    }
}


