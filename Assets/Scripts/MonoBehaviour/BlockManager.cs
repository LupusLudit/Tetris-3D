using Assets.Scripts.Blocks;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Game game;
    private GameObject[] blockPrefabs;

    private List<GameObject> currentBlockTiles = new List<GameObject>();
    private List<GameObject> predictedBlockTiles = new List<GameObject>();
    private HashSet<GameObject> placedBlocks;
    private int gridHeight;

    //TODO: instead of creating and destroying blocks, use pooling

    public void Initialize(GameExecuter gameExecuter)
    {
        game = gameExecuter.CurrentGame;
        blockPrefabs = gameExecuter.BlockPrefabs;
        placedBlocks = gameExecuter.PlacedBlocks;
        gridHeight = gameExecuter.YMax;
    }

    public void CreateNewBlock(Block block)
    {
        foreach (Vector3 tilePosition in block.TilePositions())
        {
            GameObject tile = InstantiateTile(block, ActualPosition(tilePosition));
            currentBlockTiles.Add(tile);
        }
    }

    public void CreateBlockPrediction(Block block)
    {
        int maxDrop = game.MaxPossibleDrop();
        foreach (Vector3 tilePosition in block.TilePositions())
        {
            Vector3 predictedPosition = ActualPosition(tilePosition) - Vector3.up * maxDrop;
            GameObject tile = InstantiateTile(block, predictedPosition, isPrediction: true);
            predictedBlockTiles.Add(tile);
        }
    }

    public void UpdateBlock(Block block)
    {
        int i = 0;
        foreach (Vector3 v in block.TilePositions())
        {
            currentBlockTiles[i].transform.position = ActualPosition(v);
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
            Vector3 predictedPosition = ActualPosition(v) - dropVector;
            predictedBlockTiles[i].transform.position = predictedPosition;
            i++;
        }
    }

    public void PlaceCurrentBlock()
    {
        foreach (Vector3 v in game.CurrentBlock.TilePositions())
        {
            GameObject tile = Instantiate(blockPrefabs[game.CurrentBlock.Id - 1], ActualPosition(v), Quaternion.identity);
            placedBlocks.Add(tile);
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

    private Vector3 ActualPosition(Vector3 v)
    {
        Renderer renderer = blockPrefabs[game.CurrentBlock.Id - 1].GetComponent<Renderer>();
        Vector3 cubeSize = renderer.bounds.size;
        return new Vector3(v.x + cubeSize.x / 2, gridHeight - 1 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
    }
}
