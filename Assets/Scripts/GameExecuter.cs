using Assets.Scripts;
using Assets.Scripts.Blocks;
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
    public TextMeshProUGUI ScoreText;
    public GameObject CountdownPanel;
    public TextMeshProUGUI CountdownText;

    //temp
    public LinesCompleted MessageUI;
    public GameOver GameOverUI;
    public LevelUp LevelUI;

    private Game game = new Game();
    private BlockManager blockManager;
    private Score score = new Score();
    private List<GameObject> placedBlocks = new List<GameObject>();

    private int level = 0;
    private int linesCleaned = 0;

    private readonly int maxDelay = 750;
    private readonly int minDelay = 50;
    private readonly int delayDecrease = 25;
    private int currentDelay = 750;
    private float timeSinceLastFall;

    private Dictionary<KeyCode, Action> keyActions;
    private void InitializeKeyMappings()
    {
        keyActions = new Dictionary<KeyCode, Action>
        {
            { KeyCode.UpArrow, () => game.XBack() },
            { KeyCode.DownArrow, () => game.XFoward() },
            { KeyCode.LeftArrow, () => game.ZBack() },
            { KeyCode.RightArrow, () => game.ZFoward() },
            { KeyCode.Q, () => game.RotateBlockCCW() },
            { KeyCode.E, () => game.RotateBlockCW() },
            { KeyCode.A, () => game.SwitchToDifAxis(0) },
            { KeyCode.S, () => game.SwitchToDifAxis(1) },
            { KeyCode.D, () => game.SwitchToDifAxis(2) },
            { KeyCode.C, () => 
                {
                blockManager.HoldCurrentBlock();
                DrawHeldBlock(game.HeldBlock);
                DrawNextBlock(game.Holder);
                }
            }
        };
    }

    void Start()
    {
        blockManager = new BlockManager(game, BlockPrefabs, placedBlocks);

        InitializeKeyMappings();
        StartCoroutine(CountdownCoroutine());
        blockManager.CreateNewBlock(game.CurrentBlock);
        blockManager.CreateBlockPrediction(game.CurrentBlock);
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
        if (!game.GameOver && CountdownText.gameObject.activeSelf == false)
        {
            currentDelay = 750;
            HandleKeys();
            timeSinceLastFall += Time.deltaTime;

            if (timeSinceLastFall >= currentDelay / 1000f)
            {
                game.MoveBlockDown();
                if (game.BlockPlaced) Restart();

                blockManager.UpdateBlock(game.CurrentBlock);
                blockManager.UpdatePrediction(game.CurrentBlock);

                timeSinceLastFall = 0f;
            }
        }
        else if (game.GameOver) GameOverUI.ShowEndGameScreen();
    }

    private void Restart()
    {
        blockManager.PlaceCurrentBlock();
        ClearFullLayers();
        game.NextBlock();
        blockManager.CreateNewBlock(game.CurrentBlock);
        blockManager.CreateBlockPrediction(game.CurrentBlock);
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

    private void DrawScore()
    {
        ScoreText.text = $"Score: {score.CurrentScore}";
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

            MessageUI.Message.text = score.GetMessage(numOfCleared);
            MessageUI.PlusScore.text = $"+{score.AddLayerScore(level, numOfCleared)}";
            MessageUI.ShowUI();

            DrawScore();
        }
    }

    private void CheckLevelUp()
    {
        if (linesCleaned >= 10)
        {
            level++;
            linesCleaned = 0;
            LevelUI.LevelText.text = $"LEVEL: {level}";
            LevelUI.ShowUI();
        }
    }

    /*
     * Since modifying a collection while iterating over it directly in a foreach loop is not allowed,
     * we must iteratate over a different collection, so the program won't crash.
     * We are going to use a temporary list to store the tiles that should be removed.
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            game.DropBlock();
            Restart();
        }
        else
        {
            foreach (var keyAction in keyActions)
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    keyAction.Value();
                }
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentDelay = 75;
            score.IncrementScore();
            DrawScore();
        }

        blockManager.UpdateBlock(game.CurrentBlock);
        blockManager.UpdatePrediction(game.CurrentBlock);
    }
}
