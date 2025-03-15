using System.Collections.Generic;
using UnityEngine;

public class TilePoolManager : MonoBehaviour
{
    public TilePoolConfig[] tileConfigs;
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    public static TilePoolManager Instance { get; private set; }

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

    private void AddTileToPool(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject tile = Instantiate(prefab);
        var pooledTile = tile.AddComponent<PooledTile>();
        pooledTile.prefabReference = prefab;

        tile.SetActive(false);
        tile.transform.SetParent(this.transform);
        pool.Enqueue(tile);
    }


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

    public void ReturnTile(GameObject tile)
    {
        var pooledTile = tile.GetComponent<PooledTile>();

        tile.SetActive(false);
        tile.transform.SetParent(this.transform);
        pools[pooledTile.prefabReference].Enqueue(tile);
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


