using Assets.Scripts.Blocks;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MonoBehaviour;

public class BlockManager : MonoBehaviour
{
    private Game game;
    private GameObject[] blockPrefabs;

    private List<GameObject> currentBlockTiles = new List<GameObject>();
    private List<GameObject> predictedBlockTiles = new List<GameObject>();
    private HashSet<GameObject> placedBlocks;
    private int gridHeight;
    private Renderer blockRenderer;

    //TODO: instead of creating and destroying blocks, use pooling

    public void Initialize(GameExecuter gameExecuter)
    {
        game = gameExecuter.CurrentGame;
        blockPrefabs = gameExecuter.BlockPrefabs;
        placedBlocks = gameExecuter.PlacedBlocks;
        gridHeight = gameExecuter.YMax;
        blockRenderer = blockPrefabs[game.CurrentBlock.Id - 1].GetComponent<Renderer>();
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
}
