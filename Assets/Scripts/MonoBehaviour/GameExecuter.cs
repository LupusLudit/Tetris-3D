using Assets.Scripts;
using Assets.Scripts.Blocks;
using Assets.Scripts.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameExecuter : MonoBehaviour
{
    public GameObject[] BlockPrefabs;
    public Sprite[] BlockImages;
    public Image NextImage;
    public Image HoldImage;
    public UIManager Manager;
    public GameObject CountdownPanel;
    public TextMeshProUGUI CountdownText;
    public Camera gameCamera;
    public BackgroundRenderer backgroundRenderer;

    private Vector3 boardCenter = new Vector3(5, 6.5876f, 5); //precalculated values, will be adjusted later
    private Game game = new Game();
    private BlockManager blockManager;
    private Score score = new Score();
    private List<GameObject> placedBlocks = new List<GameObject>();

    private int level = 0;
    private int linesCleaned = 0;
    
    private float timeSinceLastFall;
    private DelayManager delay = new DelayManager(750, 50, 25);
    private Dictionary<KeyCode, Action> keyActions;
    private void InitializeKeyMappings()
    {
        keyActions = new Dictionary<KeyCode, Action>
        {
            { KeyCode.UpArrow, () => game.XBack() },
            { KeyCode.DownArrow, () => game.XForward() },
            { KeyCode.LeftArrow, () => game.ZBack() },
            { KeyCode.RightArrow, () => game.ZForward() },
            { KeyCode.Q, () => game.RotateBlockCCW() },
            { KeyCode.E, () => game.RotateBlockCW() },
            { KeyCode.A, () => game.SwitchToDifAxis(0) },
            { KeyCode.S, () => game.SwitchToDifAxis(1) },
            { KeyCode.D, () => game.SwitchToDifAxis(2) },
            { KeyCode.LeftShift, () =>
                {
                    delay.CurrentDelay = 75;
                    score.IncrementScore();
                    Manager.DrawScoreUI(score.CurrentScore);
                }
            },
            { KeyCode.Space, () =>
                {
                    game.DropBlock();
                    Restart();
                }
            },
            { KeyCode.C, () =>
                {
                    blockManager.HoldCurrentBlock();
                    DrawHeldBlock(game.HeldBlock);
                    DrawNextBlock(game.Holder);
                }
            },
            { KeyCode.L, () => RotateCameraAroundBoard(-0.5f) },
            { KeyCode.K, () => RotateCameraAroundBoard(0.5f) },
            { KeyCode.R, () => backgroundRenderer.ResetToDefault() },
            { KeyCode.Escape, () => Manager.Pause() }
        };
    }

    void Start()
    {
        blockManager = new BlockManager(game, BlockPrefabs, placedBlocks);

        InitializeKeyMappings();
        StartCoroutine(CountdownCoroutine());
        blockManager.CreateNewBlock(game.CurrentBlock);
        blockManager.CreateBlockPrediction(game.CurrentBlock);
        DrawNextBlock(game.Holder);
        timeSinceLastFall = 0f;
    }

    // Countdown coroutine
    IEnumerator CountdownCoroutine()
    {
        CountdownText.gameObject.SetActive(true);

        CountdownText.text = "GET READY!";
        yield return new WaitForSeconds(1f);
        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        CountdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        CountdownText.gameObject.SetActive(false);
        CountdownPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!game.GameOver && CountdownText.gameObject.activeSelf == false && !Manager.GameMenu.IsPaused)
        {
            delay.AdjustDelay(score.CurrentScore);

            HandleKeys();
            timeSinceLastFall += Time.deltaTime;

            if (timeSinceLastFall >= delay.CurrentDelay / 1000f)
            {
                game.MoveBlockDown();
                if (game.BlockPlaced) Restart();

                blockManager.UpdateBlock(game.CurrentBlock);
                blockManager.UpdatePrediction(game.CurrentBlock);

                timeSinceLastFall = 0f;
            }
        }
        else if (game.GameOver) Manager.DrawGameOverScreen();
    }

    private void Restart()
    {
        blockManager.PlaceCurrentBlock();
        ClearFullLayers();
        game.NextBlock();
        blockManager.CreateNewBlock(game.CurrentBlock);
        blockManager.CreateBlockPrediction(game.CurrentBlock);
        DrawNextBlock(game.Holder);
        game.BlockPlaced = false;
    }

    private void DrawHeldBlock(Block heldBlock)
    {
        if (heldBlock != null)
        {
            HoldImage.sprite = BlockImages[heldBlock.Id - 1];
        }
    }

    private void DrawNextBlock(BlockHolder holder)
    {
        Block next = holder.NextBlock;
        NextImage.sprite = BlockImages[next.Id - 1];
    }

    private void ClearFullLayers()
    {
        int numOfCleared = 0;
        GameGrid grid = game.Grid;

        for (int y = game.Grid.Y - 1; y > 0; y--)
        {
            if (game.Grid.IsLayerFull(y))
            {
                ClearBlocksInRow(y);
                numOfCleared++;
                linesCleaned++;
            }
            else if (numOfCleared > 0) MoveBlocksDown(y, numOfCleared);
        }

        if (numOfCleared > 0)
        {
            CheckLevelUp();
            Manager.DrawLinesCompletedUI(score, level, numOfCleared);
            Manager.DrawScoreUI(score.CurrentScore);
        }
    }

    private void CheckLevelUp()
    {
        if (linesCleaned >= 10)
        {
            level++;
            linesCleaned = 0;
            Manager.DrawLevelUpUI(level);
        }
    }

    /*
     * Since modifying a collection while iterating over it directly in a foreach loop is not allowed,
     * it is necessary to iterate over a different collection, so the program won't crash.
     * A temporary list will be used to store the tiles that should be removed.
     */
    private void ClearBlocksInRow(int y)
    {
        game.Grid.ClearLayer(y);
        List<GameObject> tilesToRemove = new List<GameObject>();

        foreach (var tile in placedBlocks)
        {
            if (tile.transform.position.y == 21 - y + 0.5)
            {
                tilesToRemove.Add(tile);
            }
        }
        foreach (var tile in tilesToRemove)
        {
            placedBlocks.Remove(tile);
            Destroy(tile);
        }
    }

    private void MoveBlocksDown(int y, int drop)
    {
        game.Grid.MoveLayerDown(y, drop);
        Vector3 dropVector = new Vector3(0, drop, 0);
        foreach (var tile in placedBlocks)
        {
            if (tile.transform.position.y == 21 - y + 0.5)
            {
                Vector3 newPosition = tile.transform.position - dropVector;
                tile.transform.position = newPosition;
            }
        }
    }

    private void HandleKeys()
    {
        
        foreach (var keyAction in keyActions)
        {
            /*
             * The first condition checks if the desired button is being HELD and if so the assigned action will be performed
             * Or it just checks if the key has been PRESSED and if so the assigned action will be performed
             */
            if ((Input.GetKey(keyAction.Key) && IsDesiredHold(keyAction.Key)) || Input.GetKeyDown(keyAction.Key))
            {
                    keyAction.Value();
            }
        }

        blockManager.UpdateBlock(game.CurrentBlock);
        blockManager.UpdatePrediction(game.CurrentBlock);
    }

    private bool IsDesiredHold(KeyCode k)
    {
        return k == GetKeyFromIndex(9) || k == GetKeyFromIndex(12) || k == GetKeyFromIndex(13);
    }

    private void RotateCameraAroundBoard(float angle)
    {
        // Calculating the new position by rotating around the center point of the board (only on the Y axis)
        Vector3 direction = gameCamera.transform.position - boardCenter;
        direction = Quaternion.Euler(0, angle, 0) * direction; // Rotate around the Y-axis
        gameCamera.transform.position = boardCenter + direction;

        // Make the camera always look at the center of the board
        gameCamera.transform.LookAt(boardCenter);
    }

    public Dictionary<KeyCode, Action> GetKeyActions()
    {
        return keyActions;
    }

    public Action GetActionFromIndex(int index)
    {
        if (index >= 0 && index < keyActions.Count)
        {
            return new List<Action>(keyActions.Values)[index];
        }
        return null;
    }

    public KeyCode GetKeyFromIndex(int index)
    {
        if (index >= 0 && index < keyActions.Count)
        {
            return new List<KeyCode>(keyActions.Keys)[index];
        }
        return KeyCode.None;
    }
}
