using Assets.Scripts.Events;
using Assets.Scripts.Logic.Holders;
using Assets.Scripts.PowerUps;
using Assets.Scripts.Unity.ObjectPooling;
using Assets.Scripts.Unity.UI.DynamicMessages;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Unity
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUpManager"]/*'/>
    public class PowerUpManager : MonoBehaviour
    {
        public GameExecuter Executer;
        public PowerUpMessage PowerUpMessage;
        public GameObject[] PowerUpPrefabs;
        public float SpawnDelay = 10f;

        private PowerUpHolder powerUpHolder;
        private HashSet<GameObject> activePowerUps = new HashSet<GameObject>();
        private float spawnTimer = 0f;


        /// <summary>
        /// Handles spawning PowerUps periodically and checking for collected PowerUps.
        /// </summary>
        void Update()
        {
            if (Executer.IsGameActive())
            {
                if (Executer.CurrentGame?.CurrentBlock == null) return;

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

        /// <summary>
        /// Subscribes to block placement events when enabled.
        /// </summary>
        void OnEnable()
        {
            BlockEvents.OnBlockPlaced += UpdatePowerUps;
        }

        /// <summary>
        /// Unsubscribes from block placement events when disabled.
        /// </summary>
        void OnDisable()
        {
            BlockEvents.OnBlockPlaced -= UpdatePowerUps;
        }

        /// <summary>
        /// Checks if the newly placed blocks overlap with active PowerUps.
        /// Activates and removes collected PowerUps.
        /// </summary>
        /// <param name="positions">List of tile positions to check against PowerUps.</param>
        private void UpdatePowerUps(List<Vector3> positions)
        {
            if (activePowerUps.Count == 0 || positions == null || positions.Count == 0) return;

            List<GameObject> toRemove = new List<GameObject>();

            foreach (Vector3 tilePos in positions)
            {
                foreach (var powerUpObject in activePowerUps)
                {
                    var component = powerUpObject?.GetComponent<PowerUpComponent>();
                    if (component == null || component.PowerUpInstance == null) continue;

                    Vector3 powerUpPos = powerUpObject.transform.position;
                    if (powerUpPos == tilePos)
                    {
                        component.PowerUpInstance.Use();

                        PowerUpMessage.SetMessage(component.PowerUpInstance.Title, component.PowerUpInstance.Description);
                        PowerUpMessage.ShowUI();

                        toRemove.Add(powerUpObject);
                        Executer?.SoundEffects?.PlayEffect(0);
                    }
                }
            }

            foreach (var powerUpObject in toRemove)
            {
                RemovePowerUp(powerUpObject);
            }
        }

        /// <summary>
        /// Spawns a new random PowerUp into the game world.
        /// </summary>
        private void SpawnPowerUp()
        {
            PowerUp nextPowerUp = powerUpHolder.GetNextPowerUp();
            nextPowerUp.Position = GenerateRandomPosition(nextPowerUp);
            GameObject powerUpTile = InstantiatePowerUp(nextPowerUp);

            if (powerUpTile != null)
            {
                activePowerUps.Add(powerUpTile);
            }
        }

        /// <summary>
        /// Generates a random unoccupied position for a PowerUp to spawn.
        /// </summary>
        /// <param name="powerUp">The PowerUp that needs a position assigned.</param>
        /// <returns>A Vector3 position for spawning.</returns>
        private Vector3 GenerateRandomPosition(PowerUp powerUp)
        {
            Vector3 position;
            System.Random random = new();

            int attempts = 0;
            do
            {
                position = new Vector3(
                    random.Next(Executer.XMax),
                    random.Next(Mathf.Max(1, Executer.YMax - 2)),
                    random.Next(Executer.ZMax)
                );

                powerUp.Position = position;
                attempts++;
                if (attempts > 50) break;
            }
            while (PositionInPlacedBlocks(powerUp));

            return position;
        }

        /// <summary>
        /// Checks if the desired PowerUp spawn position is already occupied by a block.
        /// </summary>
        /// <param name="powerUp">PowerUp instance with a target position.</param>
        /// <returns>True if position is occupied, false otherwise.</returns>
        private bool PositionInPlacedBlocks(PowerUp powerUp)
        {
            foreach (var tile in Executer.Manager.PlacedBlocks)
            {
                if (tile.transform.position == powerUp.Position) return true;
            }

            return false;
        }

        /// <summary>
        /// Instantiates a PowerUp GameObject and attaches its data.
        /// </summary>
        /// <param name="powerUp">PowerUp data to initialize.</param>
        /// <returns>Instantiated GameObject with the PowerUpComponent attached.</returns>
        private GameObject InstantiatePowerUp(PowerUp powerUp)
        {
            int index = Mathf.Clamp(powerUp.Id - 1, 0, PowerUpPrefabs.Length - 1);
            GameObject powerUpObject = TilePoolManager.Instance.GetTile(PowerUpPrefabs[index]);
            powerUpObject.transform.position = powerUp.Position;
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

    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUpComponent"]/*'/>
    public class PowerUpComponent : MonoBehaviour
    {
        public PowerUp PowerUpInstance { get; private set; }

        public void Initialize(PowerUp powerUp)
        {
            PowerUpInstance = powerUp;
        }
    }
}
