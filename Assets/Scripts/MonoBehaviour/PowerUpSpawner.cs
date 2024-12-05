using Assets.Scripts;
using Assets.Scripts.Logic;
using Assets.Scripts.MonoBehaviour;
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
    private Renderer powerUpRenderer;
    void Update()
    {
        if (!executer.CurrentGame.GameOver)
        {
            // Lazy initialization of PowerUpHolder => we need to initialize the powerUpHolder after the Grid
            if (powerUpHolder == null && executer.CurrentGame.Grid != null)
            {
                powerUpRenderer = executer.BlockPrefabs[executer.CurrentGame.CurrentBlock.Id - 1].GetComponent<Renderer>();
                powerUpHolder = new PowerUpHolder(executer, powerUpRenderer);
            }

            if (powerUpHolder != null)
            {
                UpdatePowerUps();

                spawnTimer += Time.deltaTime;
                if (spawnTimer >= 5f) // Temporarily set to 10
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

        foreach (Vector3 tilePosition in executer.CurrentGame.CurrentBlock.TilePositions())
        {
            Vector3 pos = PositionConvertor.ActualTilePosition(tilePosition,
                executer.BlockPrefabs[executer.CurrentGame.CurrentBlock.Id - 1].GetComponent<Renderer>(),
                executer.YMax);

            // Stores all colliders that overlap the sphere inside the tile position
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.1f);

            foreach (Collider collider in hitColliders)
            {
                //Checks if the colliders gameObject has a powerUpComponent and returns reference to it
                if (collider.gameObject.TryGetComponent(out PowerUpComponent powerUpComponent))
                {
                    PowerUp powerUp = powerUpComponent.PowerUpInstance;
                    powerUp.Use();
                    Debug.Log($"Used a power-up with id {powerUp.Id}");
                    RemovePowerUp(collider.gameObject);
                }
            }
        }
    }

    private void SpawnPowerUp()
    {
        PowerUp nextPowerUp = powerUpHolder.GetNextPowerUp();
        InstantiatePowerUp(nextPowerUp);
    }

    private void InstantiatePowerUp(PowerUp powerUp)
    {
        GameObject powerUpObject = Instantiate(PowerUpPrefabs[powerUp.Id - 1],
            PositionConvertor.PowerUpPosition(powerUp, PowerUpPrefabs[powerUp.Id - 1].GetComponent<Renderer>(), executer.YMax),
            Quaternion.identity);

        activePowerUps.Add(powerUpObject);

        PowerUpComponent component = powerUpObject.AddComponent<PowerUpComponent>();
        component.Initialize(powerUp);

        Collider collider = powerUpObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
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