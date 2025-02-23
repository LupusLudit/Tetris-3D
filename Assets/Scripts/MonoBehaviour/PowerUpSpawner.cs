using Assets.Scripts.Logic;
using Assets.Scripts.MonoBehavior;
using Assets.Scripts.PowerUps;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameExecuter Executer;
    public PowerUpMessage PowerUpMessage;
    public GameObject[] PowerUpPrefabs;

    private PowerUpHolder powerUpHolder;
    private HashSet<GameObject> activePowerUps = new HashSet<GameObject>();
    private HashSet<GameObject> processedPowerUps = new HashSet<GameObject>();
    private float spawnTimer = 0f;
    private Renderer powerUpRenderer;
   

    void Update()
    {
        if (Executer.IsGameActive())
        {
            // Lazy initialization of PowerUpHolder => we need to initialize the powerUpHolder after the Grid
            if (powerUpHolder == null && Executer.CurrentGame.Grid != null)
            {
                powerUpRenderer = Executer.BlockPrefabs[Executer.CurrentGame.CurrentBlock.Id - 1].GetComponent<Renderer>();
                powerUpHolder = new PowerUpHolder(Executer, powerUpRenderer);
            }

            if (powerUpHolder != null)
            {
                UpdatePowerUps();

                spawnTimer += Time.deltaTime;
                if (spawnTimer >= 10f) // Temporarily set to 10
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

        processedPowerUps.Clear(); // Clear the processed set for the current frame

        foreach (Vector3 tilePosition in Executer.CurrentGame.CurrentBlock.TilePositions())
        {
            Vector3 pos = PositionConvertor.ActualTilePosition(tilePosition,
                Executer.BlockPrefabs[Executer.CurrentGame.CurrentBlock.Id - 1].GetComponent<Renderer>(),
                Executer.YMax);

            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.1f);

            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.TryGetComponent(out PowerUpComponent powerUpComponent))
                {
                    GameObject powerUpObject = collider.gameObject;

                    // Skip if this power-up has already been processed
                    if (processedPowerUps.Contains(powerUpObject)) continue;

                    // Process the power-up
                    PowerUp powerUp = powerUpComponent.PowerUpInstance;
                    powerUp.Use();
                    PowerUpMessage.SetMessage(powerUp.Title, powerUp.Description);
                    PowerUpMessage.ShowUI();
                    Debug.Log($"Used a power-up with id {powerUp.Id}");
                    RemovePowerUp(powerUpObject);

                    // Mark as processed
                    processedPowerUps.Add(powerUpObject);
                    //Play effect
                    Executer.SoundEffects.PlayEffect(0);
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
            PositionConvertor.PowerUpPosition(powerUp, powerUpRenderer, Executer.YMax),
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
