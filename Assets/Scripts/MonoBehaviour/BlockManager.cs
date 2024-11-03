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
    private List<GameObject> placedBlocks;
    private int gridHeight;

    public BlockManager(Game game, GameObject[] blockPrefabs, List<GameObject> placedBlocks, int gridHeight)
    {
        this.game = game;
        this.blockPrefabs = blockPrefabs;
        this.placedBlocks = placedBlocks;
        this.gridHeight = gridHeight;
    }

    public void CreateNewBlock(Block block)
    {
        foreach (Vector3 v in block.TilePositions())
        {
            GameObject tile = Instantiate(blockPrefabs[block.Id - 1], ActualPosition(v), Quaternion.identity);
            currentBlockTiles.Add(tile);
        }
    }

    public void CreateBlockPrediction(Block block)
    {
        foreach (Vector3 v in block.TilePositions())
        {
            int drop = game.MaxPossibleDrop();
            Vector3 predictedPosition = ActualPosition(v) - new Vector3(0, drop, 0);
            GameObject tile = Instantiate(blockPrefabs[block.Id - 1], predictedPosition, Quaternion.identity);
            tile.transform.localScale *= 0.999f;
            tile.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
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
        ClearList(predictedBlockTiles);
        ClearList(currentBlockTiles);
    }

    public void HoldCurrentBlock()
    {
        ClearList(predictedBlockTiles);
        ClearList(currentBlockTiles);

        game.HoldBlock();

        CreateBlockPrediction(game.CurrentBlock);
        CreateNewBlock(game.CurrentBlock);

    }

    private void ClearList(List<GameObject> list)
    {
        foreach (var tile in list)
        {
            Destroy(tile);
        }
        list.Clear();
    }

    /*
     * Since our cubes are always 1x1x1 and the position of the cube refers to the center of the cube,
     * we have to move the cube by +0.5(-0.5+1 since the positions in unity are shifted) on each axis in order for it to be positioned properly
    */
    private Vector3 ActualPosition(Vector3 v)
    {
        Renderer renderer = blockPrefabs[game.CurrentBlock.Id - 1].GetComponent<Renderer>();
        Vector3 cubeSize = renderer.bounds.size;
        return new Vector3(v.x + cubeSize.x / 2, gridHeight-1 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
    }

}