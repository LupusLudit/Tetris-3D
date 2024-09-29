using Assets.Scripts;
using Assets.Scripts.Blocks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameExecuter : MonoBehaviour
{
    public GameObject[] BlockPrefabs;
    public Sprite[] BlockImages;
    public Image NextImage;
    public Image HoldImage;
    public TextMeshProUGUI ScoreText;
    public LinesCompleted MessageUI;
    public GameOver GameOverUI;

    private Game game = new Game();
    private Score score = new Score();
    private int level = 0;
    private int linesCleaned = 0;
    private readonly int maxDelay = 750;
    private readonly int minDelay = 50;
    private readonly int delayDecrease = 25;
    private int currentDelay = 750;

    private List<GameObject> currentBlockTiles = new List<GameObject>();
    private List<GameObject> predictedBlockTiles = new List<GameObject>();
    private List<GameObject> placedBlocks = new List<GameObject>();

    private float timeSinceLastFall;  // Track the time since the block last moved down

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
            { KeyCode.C, () => HoldBlock(game.CurrentBlock) }
        };
    }

    void Start()
    {
        InitializeKeyMappings();
        CreateNewBlock(game.CurrentBlock);
        CreateBlockPrediction(game.CurrentBlock);
        timeSinceLastFall = 0f;
    }

    void Update()
    {
        if (!game.GameOver)
        {
            currentDelay = 750;
            HandleKeys();
            timeSinceLastFall += Time.deltaTime;

            if (timeSinceLastFall >= currentDelay / 1000f)
            {
                game.MoveBlockDown();
                if (game.BlockPlaced) Restart();
                else
                {
                    UpdateBlock(game.CurrentBlock);
                    UpdatePrediction(game.CurrentBlock);
                }

                timeSinceLastFall = 0f;
            }
        }
        else
        {
            GameOverUI.ShowEndGameScreen();
        }
    }


    private void Restart()
    {
        PlaceCurrentBlock();
        game.NextBlock();
        CreateNewBlock(game.CurrentBlock);
        CreateBlockPrediction(game.CurrentBlock);
        game.BlockPlaced = false;
    }

    private void CreateNewBlock(Block block)
    {
        foreach (Vector3 v in block.TilePositions())
        {
            GameObject tile = Instantiate(BlockPrefabs[block.Id - 1], ActualPosition(v), Quaternion.identity);
            currentBlockTiles.Add(tile);
        }
        DrawNextBlock(game.Holder);
    }

    private void CreateBlockPrediction(Block block)
    {
        foreach (Vector3 v in block.TilePositions())
        {
            int drop = game.MaxPossibleDrop();
            Vector3 dropVector = new Vector3(0, drop, 0);
            Vector3 predictedPosition = ActualPosition(v) - dropVector;
            GameObject tile = Instantiate(BlockPrefabs[block.Id - 1], predictedPosition, Quaternion.identity);

            /*
             * Scale the tile to 99.9% of its original size,
             * meaning that if the original block comes in contact with it,
             * it will overlay the prediction
             */

            tile.transform.localScale = tile.transform.localScale * 0.999f;
            Renderer renderer = tile.GetComponent<Renderer>();

            // Seting the material of the prediciton to be a different color
            renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
            predictedBlockTiles.Add(tile);
        }
    }


    private void UpdateBlock(Block block)
    {
        int i = 0;
        foreach (Vector3 v in block.TilePositions())
        {
            currentBlockTiles[i].transform.position = ActualPosition(v);
            i++;
        }
    }

    private void UpdatePrediction(Block block)
    {
        int i = 0;
        foreach (Vector3 v in block.TilePositions())
        {
            int drop = game.MaxPossibleDrop();
            Vector3 dropVector = new Vector3(0, drop, 0);
            Vector3 predictedPosition = ActualPosition(v) - dropVector;
            predictedBlockTiles[i].transform.position = predictedPosition;
            i++;
        }
    }

    private void HoldBlock(Block block)
    {
        ClearList(predictedBlockTiles);
        ClearList(currentBlockTiles);

        game.HoldBlock();

        CreateBlockPrediction(game.CurrentBlock);
        CreateNewBlock(game.CurrentBlock);

        DrawHeldBlock(game.HeldBlock);
        DrawNextBlock(game.Holder);

    }

    private void PlaceCurrentBlock()
    {
        foreach (Vector3 v in game.CurrentBlock.TilePositions())
        {
            GameObject tile = Instantiate(BlockPrefabs[game.CurrentBlock.Id - 1], ActualPosition(v), Quaternion.identity);
            placedBlocks.Add(tile);
        }
        ClearList(predictedBlockTiles);
        ClearList(currentBlockTiles);

        ClearFullLayers();
    }

    /*
     * Since our cubes are always 1x1x1 and the position of the cube referes to the center of the cube,
     * we have to move the cube by +0.5(-0.5+1 since the positions in unity are shifted) on each axis in order for it to be positioned properly
    */
    private Vector3 ActualPosition(Vector3 v)
    {
        // Get the size of the prefab object
        Renderer renderer = BlockPrefabs[game.CurrentBlock.Id - 1].GetComponent<Renderer>();
        Vector3 cubeSize = renderer.bounds.size;

        // Calculate half the size for each dimension
        float halfX = cubeSize.x / 2;
        float halfY = cubeSize.y / 2;
        float halfZ = cubeSize.z / 2;

        return new Vector3(v.x + halfX, 21 - v.y + halfY, v.z + halfZ);
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

    private void ClearList(List<GameObject> list)
    {
        foreach (var tile in list)
        {
            Destroy(tile);
        }
        list.Clear();
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

        UpdateBlock(game.CurrentBlock);
        UpdatePrediction(game.CurrentBlock);
    }
}
