using Assets.Scripts.Blocks;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MonoBehaviour;
using Assets.Scripts.Logic;

public class GameManager : MonoBehaviour
{

    public const float RotationAngle = 0.6f;
    public HashSet<GameObject> PlacedBlocks { get; private set; } = new HashSet<GameObject>();
    public bool DoubleScore { get; set; } = false;
    public bool Freezed { get; set; } = false;
    public bool LimitedMovement { get; set; } = false;
    public int ClearedLayers { get; private set; } = 0;

    private Game game;
    private GameObject[] blockPrefabs;
    private List<GameObject> currentBlockTiles = new List<GameObject>();
    private List<GameObject> predictedBlockTiles = new List<GameObject>();
    private Renderer blockRenderer;
    private UIManager ui;
    private ImageDrawer imageDrawer;
    private Camera gameCamera;
    private int gridHeight;

    private Vector3 lookPoint;
    private Score score;
    private DelayManager delay;
    private int level = 0;
    private int linesCleaned = 0;
    //TODO: instead of creating and destroying blocks, use pooling

    public void Initialize(GameExecuter gameExecuter)
    {
        game = gameExecuter.CurrentGame;
        blockPrefabs = gameExecuter.BlockPrefabs;
        gridHeight = gameExecuter.YMax;
        blockRenderer = gameExecuter.BlockPrefabs[game.CurrentBlock.Id - 1].GetComponent<Renderer>();
        ui = gameExecuter.UI;
        imageDrawer = gameExecuter.ImageDrawer;
        score = gameExecuter.Score;
        gameCamera = gameExecuter.GameCamera;
        lookPoint = gameExecuter.LookPoint;
    }

    public void CreateNewBlock(Block block)
    {
        foreach (Vector3 tilePosition in block.TilePositions())
        {
            GameObject tile = InstantiateTile(block, PositionConvertor.ActualTilePosition(tilePosition, blockRenderer, gridHeight));
            currentBlockTiles.Add(tile);
        }
    }

    public void CreateBlockPrediction(Block block)
    {
        int maxDrop = game.MaxPossibleDrop();
        foreach (Vector3 tilePosition in block.TilePositions())
        {
            Vector3 predictedPosition = PositionConvertor.ActualTilePosition(tilePosition, blockRenderer, gridHeight) - Vector3.up * maxDrop;
            GameObject tile = InstantiateTile(block, predictedPosition, isPrediction: true);
            predictedBlockTiles.Add(tile);
        }
    }

    public void UpdateBlock(Block block)
    {
        int i = 0;
        foreach (Vector3 v in block.TilePositions())
        {
            currentBlockTiles[i].transform.position = PositionConvertor.ActualTilePosition(v, blockRenderer, gridHeight);
            i++;
        }
    }

    public void UpdatePrediction(Block block)
    { 
        int i = 0;
        foreach (Vector3 v in block.TilePositions())
        {
            int drop = game.MaxPossibleDrop();
            Vector3 dropVector = new Vector3(0, drop, 0);
            Vector3 predictedPosition = PositionConvertor.ActualTilePosition(v, blockRenderer, gridHeight) - dropVector;
            predictedBlockTiles[i].transform.position = predictedPosition;
            i++;
        }
    }

    public void PlaceCurrentBlock()
    {
        foreach (Vector3 v in game.CurrentBlock.TilePositions())
        {
            GameObject tile = Instantiate(blockPrefabs[game.CurrentBlock.Id - 1],
                PositionConvertor.ActualTilePosition(v, blockRenderer, gridHeight),
                Quaternion.identity);
            PlacedBlocks.Add(tile);
        }
        ClearCurrentBlocks();
    }

    public void HoldCurrentBlock()
    {
        ClearCurrentBlocks();

        game.HoldBlock();

        CreateBlockPrediction(game.CurrentBlock);
        CreateNewBlock(game.CurrentBlock);
    }
    private GameObject InstantiateTile(Block block, Vector3 position, bool isPrediction = false)
    {
        GameObject tile = Instantiate(blockPrefabs[block.Id - 1], position, Quaternion.identity);

        if (isPrediction)
        {
            tile.transform.localScale *= 0.999f;
            Renderer renderer = tile.GetComponent<Renderer>();
            renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
        }
        return tile;
    }

    private void ClearList(List<GameObject> list)
    {
        foreach (var tile in list)
        {
            Destroy(tile);
        }
        list.Clear();
    }

    public void ClearCurrentBlocks()
    {
        ClearList(predictedBlockTiles);
        ClearList(currentBlockTiles);
    }

    public void ClearFullLayers()
    {
        ClearedLayers = 0;
        for (int y = game.Grid.Y - 1; y > 0; y--)
        {
            if (game.Grid.IsLayerFull(y))
            {
                ClearBlocksInRow(y);
                ClearedLayers++;
                linesCleaned++;
            }
            else if (ClearedLayers > 0)
            {
                MoveBlocksDown(y, ClearedLayers);
            }
        }

        if (ClearedLayers > 0)
        {
            CheckLevelUp();
            ui.DrawLinesCompletedUI(score, level, ClearedLayers, DoubleScore);
            ui.DrawScoreUI(score.CurrentScore);
        }
    }

    private void CheckLevelUp()
    {
        if (linesCleaned >= 10)
        {
            level++;
            linesCleaned = 0;
            ui.DrawLevelUpUI(level);
        }
    }

    //We cant remove elements from a collection while iterating over it, so we first load tiles into the tilesToRemove List.
    public void ClearBlocksInRow(int y)
    {
        game.Grid.ClearLayer(y);
        var tilesToRemove = new List<GameObject>();

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.y == gridHeight - 1 - y + 0.5f)
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            PlacedBlocks.Remove(tile);
            Destroy(tile);
        }
    }

    public void ClearBlocksInColumn(int x, int z)
    {
        game.Grid.ClearColumn(x, z);
        var tilesToRemove = new List<GameObject>();

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.x == x + 0.5f && tile.transform.position.z == z + 0.5f)
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            PlacedBlocks.Remove(tile);
            Destroy(tile);
        }
    }

    public void RemoveTile(GameObject tile)
    {
        Destroy(tile);
    }

    public void MoveBlocksDown(int y, int drop)
    {
        game.Grid.MoveLayerDown(y, drop);
        Vector3 dropVector = new Vector3(0, drop, 0);

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.y == gridHeight - 1 - y + 0.5f)
            {
                tile.transform.position -= dropVector;
            }
        }
    }

    public void NextWithoutPlacing()
    {
        ClearCurrentBlocks();
        game.NextBlock();
        CreateNewBlock(game.CurrentBlock);
        CreateBlockPrediction(game.CurrentBlock);
        imageDrawer.DrawNextBlock(game.Holder);
    }

    public void RotateCamera(float angle)
    {
        Vector3 direction = gameCamera.transform.position - lookPoint;
        direction = Quaternion.Euler(0, angle, 0) * direction;
        gameCamera.transform.position = lookPoint + direction;
        gameCamera.transform.LookAt(lookPoint);
    }
}
