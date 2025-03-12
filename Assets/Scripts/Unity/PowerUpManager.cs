using Assets.Scripts.Events;
using Assets.Scripts.Logic;
using Assets.Scripts.PowerUps;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameExecuter Executer;
    public PowerUpMessage PowerUpMessage;
    public GameObject[] PowerUpPrefabs;
    public float SpawnDelay = 10f;

    private PowerUpHolder powerUpHolder;
    private HashSet<GameObject> activePowerUps = new HashSet<GameObject>();
    private float spawnTimer = 0f;
   

    void Update()
    {
        if (Executer.IsGameActive())
        {
            // Lazy initialization of PowerUpHolder => we need to initialize the powerUpHolder after the Grid
            if (powerUpHolder == null && Executer.CurrentGame.Grid != null)
            {
                powerUpHolder = new PowerUpHolder(Executer);
            }

            if (powerUpHolder != null)
            {

                UpdatePowerUps(Executer.CurrentGame.CurrentBlock.TilePositions().ToList());

                spawnTimer += Time.deltaTime;
                if (spawnTimer >= SpawnDelay) // Temporarily set to 10
                {
                    spawnTimer = 0f;
                    SpawnPowerUp();
                }
            }
        }
    }

    private void UpdatePowerUps(List<Vector3> positions)
    {
        if (activePowerUps.Count == 0) return;

        foreach (Vector3 v in positions)
        {
            Vector3 tilePos = PositionConvertor.ActualTilePosition(v, Executer, Executer.YMax);

            List<GameObject> toRemove = new List<GameObject>();

            foreach (var powerUpObject in activePowerUps)
            {
                PowerUp powerUp = powerUpObject.GetComponent<PowerUpComponent>().PowerUpInstance;
                Vector3 powerUpPos = powerUpObject.transform.position;
                if (powerUpPos == tilePos)
                {
                    powerUp.Use();
                    PowerUpMessage.SetMessage(powerUp.Title, powerUp.Description);
                    PowerUpMessage.ShowUI();
                    toRemove.Add(powerUpObject);
                    Executer.SoundEffects.PlayEffect(0);
                }
            }

            // Remove outside the loop
            foreach (var powerUpObject in toRemove)
            {
                RemovePowerUp(powerUpObject);
            }

        }
    }

    void OnEnable()
    {
        BlockEvents.OnBlockPlaced += UpdatePowerUps;
    }

    void OnDisable()
    {
        BlockEvents.OnBlockPlaced -= UpdatePowerUps;
    }

    private void SpawnPowerUp()
    {
        PowerUp nextPowerUp = powerUpHolder.GetNextPowerUp();
        nextPowerUp.Position = GenerateRandomPosition(nextPowerUp);
        GameObject powerUpTile =  InstantiatePowerUp(nextPowerUp);
        activePowerUps.Add(powerUpTile);
    }
    private Vector3 GenerateRandomPosition(PowerUp powerUp)
    {
        Vector3 position;
        System.Random random = new();
        do
        {
            position = new Vector3(random.Next(Executer.XMax), random.Next(Executer.YMax - 2), random.Next(Executer.ZMax));
        }
        while (PositionInPlacedBlocks(powerUp));

        return position;
    }

    private bool PositionInPlacedBlocks(PowerUp powerUp)
    {
        foreach (var tile in Executer.Manager.PlacedBlocks)
        {
            if (tile.transform.position == PositionConvertor.PowerUpPosition(powerUp, Executer, Executer.YMax)) return true;
        }

        return false;
    }

    private GameObject InstantiatePowerUp(PowerUp powerUp)
    {
        GameObject powerUpObject = Instantiate(PowerUpPrefabs[powerUp.Id - 1],
            PositionConvertor.PowerUpPosition(powerUp, Executer, Executer.YMax),
            Quaternion.identity);
        powerUpObject.transform.localScale *= 0.9f;

        PowerUpComponent component = powerUpObject.AddComponent<PowerUpComponent>();
        component.Initialize(powerUp);

        return powerUpObject;
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
