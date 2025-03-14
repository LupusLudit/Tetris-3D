using Assets.Scripts.Blocks;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Logic;
using System.Linq;
using Assets.Scripts.Events;

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
    private UIManager ui;
    private ImageDrawer imageDrawer;
    private Camera gameCamera;
    private int gridHeight;
    private GameExecuter executer;

    private Vector3 lookPoint;
    private Score score;
    private DelayManager delay;
    private int level = 0;
    private int linesCleaned = 0;
    //TODO: instead of creating and destroying blocks, use pooling

    public void Initialize(GameExecuter gameExecuter)
    {
        executer = gameExecuter;
        game = gameExecuter.CurrentGame;
        blockPrefabs = gameExecuter.BlockPrefabs;
        gridHeight = gameExecuter.YMax;
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
            GameObject tile = InstantiateTile(block, tilePosition);
            currentBlockTiles.Add(tile);
        }
    }

    public void CreateBlockPrediction(Block block)
    {
        int maxDrop = game.MaxPossibleDrop();
        foreach (Vector3 tilePosition in block.TilePositions())
        {
            Vector3 predictedPosition = tilePosition - Vector3.up * maxDrop;
            GameObject tile = InstantiateTile(block, predictedPosition, isPrediction: true);
            predictedBlockTiles.Add(tile);
        }
    }

    public void UpdateBlock(Block block)
    {
        foreach (var (tile, position) in currentBlockTiles.Zip(block.TilePositions(), (tile, position) => (tile, position)))
        {
            tile.transform.position = position;
        }
    }

    public void UpdatePrediction(Block block)
    {
        int drop = game.MaxPossibleDrop();
        Vector3 dropVector = new Vector3(0, drop, 0);

        foreach (var (tile, position) in predictedBlockTiles.Zip(block.TilePositions(), (tile, pos) => (tile, pos)))
        {
            tile.transform.position = position - dropVector;
        }
    }

    public void PlaceCurrentBlock()
    {
        List<Vector3> placedPositions = new List<Vector3>();
        foreach (Vector3 v in game.CurrentBlock.TilePositions())
        {
            GameObject tile = InstantiateTile(game.CurrentBlock, v);
            PlacedBlocks.Add(tile);
            placedPositions.Add(v);
        }
        BlockEvents.RaiseBlockPlaced(placedPositions);

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
        GameObject tile = TilePoolManager.Instance.GetTile(blockPrefabs[block.Id - 1]);
        tile.transform.position = position;

        Renderer renderer = tile.GetComponent<Renderer>();
        var pooledTile = tile.GetComponent<PooledTile>();

        tile.transform.localScale = Vector3.one/2;
        renderer.material.color = pooledTile.defaultColor;

        if (isPrediction)
        {
            tile.transform.localScale *= 0.98f;
            renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
        }

        return tile;
    }

    private void ClearList(List<GameObject> list)
    {
        foreach (var tile in list)
        {
            TilePoolManager.Instance.ReturnTile(tile);
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
        for (int y = game.Grid.Y - 1; y >= 0; y--)
        {
            if (game.Grid.IsLayerFull(game.Grid.Y - 1 - y))
            {
                ClearBlocksInRow(game.Grid.Y - 1 - y);
                ClearedLayers++;
                linesCleaned++;
            }
            else if (ClearedLayers > 0)
            {
                MoveBlocksDown(game.Grid.Y - 1 - y, ClearedLayers);
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
            if (tile.transform.position.y == y)
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            PlacedBlocks.Remove(tile);
            TilePoolManager.Instance.ReturnTile(tile);
        }
    }

    public void ClearBlocksInColumn(int x, int z)
    {
        game.Grid.ClearColumn(x, z);
        var tilesToRemove = new List<GameObject>();

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.x == x && tile.transform.position.z == z)
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            PlacedBlocks.Remove(tile);
            TilePoolManager.Instance.ReturnTile(tile);
        }
    }

    public void MoveBlocksDown(int y, int drop)
    {
        game.Grid.MoveLayerDown(y, drop);
        Vector3 dropVector = new Vector3(0, drop, 0);

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.y == y)
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
