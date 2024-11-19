using Assets.Scripts;
using Assets.Scripts.Logic;
using Assets.Scripts.PowerUps;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameExecuter executer;
    public GameObject[] PowerUpPrefabs;

    private PowerUpHolder powerUpHolder;
    private HashSet<GameObject> activePowerUps = new HashSet<GameObject>();
    private float spawnTimer = 0f;

    void Update()
    {
        if (!executer.CurrentGame.GameOver)
        {
            // Lazy initialization of PowerUpHolder => we need to initialize the powerUpHolder after the Grid
            if (powerUpHolder == null && executer.CurrentGame.Grid != null)
            {
                powerUpHolder = new PowerUpHolder(executer);
            }

            // Update PowerUps if PowerUpHolder is initialized
            if (powerUpHolder != null)
            {
                UpdatePowerUps();

                spawnTimer += Time.deltaTime;
                if (spawnTimer >= 10f)
                {
                    spawnTimer = 0f;
                    SpawnPowerUp();
                }
            }
        }
    }

    private void UpdatePowerUps()
    {
        if (activePowerUps.Count == 0) return;

        // Temporary list to store power-ups to remove
        List<GameObject> toRemove = new List<GameObject>();

        foreach (Vector3 tilePosition in executer.CurrentGame.CurrentBlock.TilePositions())
        {
            foreach (GameObject powerUpObject in activePowerUps)
            {
                PowerUp powerUp = powerUpObject.GetComponent<PowerUpComponent>()?.PowerUpInstance;
                if (ActualTilePosition(tilePosition) == PowerUpPosition(powerUp))
                {
                    powerUp.Use();
                    Debug.Log($"Used a power up with id {powerUp.Id}");
                    toRemove.Add(powerUpObject);
                    break;
                }
            }
        }

        // Remove marked power-ups after iteration
        foreach (GameObject powerUp in toRemove)
        {
            RemovePowerUp(powerUp);
        }
    }


    private void SpawnPowerUp()
    {
        PowerUp nextPowerUp = powerUpHolder.GetNextPowerUp();
        InstantiatePowerUp(nextPowerUp);
    }

    private void InstantiatePowerUp(PowerUp powerUp)
    {
        GameObject prefab = PowerUpPrefabs[powerUp.Id - 1];

        GameObject powerUpObject = Instantiate(prefab, PowerUpPosition(powerUp), Quaternion.identity);
        activePowerUps.Add(powerUpObject);

        PowerUpComponent component = powerUpObject.AddComponent<PowerUpComponent>();
        component.Initialize(powerUp);
    }

    private Vector3 PowerUpPosition(PowerUp powerUp)
    {
        Vector3 v = powerUp.Position;
        Renderer renderer = PowerUpPrefabs[powerUp.Id - 1].GetComponent<Renderer>();
        Vector3 cubeSize = renderer.bounds.size;
        return new Vector3(v.x + cubeSize.x / 2, executer.YMax - 3 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
    }

    private Vector3 ActualTilePosition(Vector3 v)
    {
        Renderer renderer = executer.BlockPrefabs[executer.CurrentGame.CurrentBlock.Id - 1].GetComponent<Renderer>();
        Vector3 cubeSize = renderer.bounds.size;
        return new Vector3(v.x + cubeSize.x / 2, executer.YMax - 1 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
    }

    private void RemovePowerUp(GameObject powerUp)
    {
        activePowerUps.Remove(powerUp);
        Destroy(powerUp);
    }
}


public class PowerUpComponent : MonoBehaviour
{
    public PowerUp PowerUpInstance { get; private set; }

    public void Initialize(PowerUp powerUp)
    {
        PowerUpInstance = powerUp;
    }
}

