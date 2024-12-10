using Assets.Scripts;
using Assets.Scripts.Blocks;
using Assets.Scripts.Logic;
using Assets.Scripts.MonoBehaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameExecuter : MonoBehaviour
{
    [Header("Block Settings")]
    public GameObject[] BlockPrefabs;
    public Sprite[] BlockImages;

    [Header("UI Elements")]
    public Image NextImage;
    public Image HoldImage;
    public UIManager Manager;
    public GameObject CountdownPanel;
    public TextMeshProUGUI CountdownText;

    [Header("Game Settings")]
    public Camera GameCamera;
    public BackgroundRenderer BackgroundRenderer;
    public int XMax, YMax, ZMax;

    public Game CurrentGame { get; private set; }
    public HashSet<GameObject> PlacedBlocks { get; private set; } = new HashSet<GameObject>();
    public Dictionary<KeyCode, Action> keyActions { get; private set; }

    //TODO: Think of a different system, so there is no need to use so many bools
    public double DelayMultiplier { get; set; } = 1;
    public bool DoubleScore { get; set; } = false;
    public bool Freezed { get; set; } = false;
    public bool LimitedMovement { get; set; } = false;

    private BlockManager blockManager;
    private Queue<Action> actionQueue = new Queue<Action>();
    private Vector3 lookPoint;
    private Score score;
    private DelayManager delay;
    private float timeSinceLastFall;
    private int level = 0;
    private int linesCleaned = 0;
    private const float RotationAngle = 0.6f;

    KeyCode[] keys;
    private Action[] actions;
    private void InitializeActions()
    {
        actions = new Action[]
        {
        () => { if (!LimitedMovement) { CurrentGame.XBack(); } },
        () => { if (!LimitedMovement) { CurrentGame.XForward(); } },
        () => { if (!LimitedMovement) { CurrentGame.ZBack(); } },
        () => { if (!LimitedMovement) { CurrentGame.ZForward(); } },
        () => CurrentGame.RotateBlockCCW(),
        () => CurrentGame.RotateBlockCW(),
        () => CurrentGame.SwitchToDifAxis(0),
        () => CurrentGame.SwitchToDifAxis(1),
        () => CurrentGame.SwitchToDifAxis(2),
        () => { if (!Freezed) { AdjustScoreAndDelay(); } },
        () => DropAndRestart(),
        () => HoldAndDrawBlocks(),
        () => RotateCamera(RotationAngle),
        () => RotateCamera(-RotationAngle),
        () => BackgroundRenderer.ResetToDefault(),
        () => Manager.Pause()
        };
    }

    void Start()
    {
        CurrentGame = new Game(XMax, YMax, ZMax);
        blockManager = new BlockManager();
        blockManager.Initialize(this);
        lookPoint = BoardDimensions.calcLookPoint(new Vector3(XMax, YMax, ZMax), GameCamera); 

        score = new Score();
        delay = new DelayManager(750, 50, 25);
        SetKeyMappingDefault();
        InitializeActions();
        LoadSettings();
        StartCoroutine(CountdownCoroutine());

        blockManager.CreateNewBlock(CurrentGame.CurrentBlock);
        blockManager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        DrawNextBlock(CurrentGame.Holder);

        timeSinceLastFall = 0f;
    }

    void Update()
    {
        if (!IsGameActive())
        {
            if (CurrentGame.GameOver) Manager.DrawGameOverScreen();
            return;
        }

        UpdateFallDelay();
        HandleKeyInputs();

        if (timeSinceLastFall >= delay.CurrentDelay / 1000f)
        {
            ExecuteQueuedActions();
            ExecuteGameStep();
            timeSinceLastFall = 0f;
        }
    }

    private void ExecuteGameStep()
    {
        if(!Freezed) CurrentGame.MoveBlockDown();
        if (CurrentGame.BlockPlaced) RestartGameCycle();

        blockManager.UpdateBlock(CurrentGame.CurrentBlock);
        blockManager.UpdatePrediction(CurrentGame.CurrentBlock);
    }

    private void RestartGameCycle()
    {
        blockManager.PlaceCurrentBlock();
        ClearFullLayers();
        CurrentGame.NextBlock();
        blockManager.CreateNewBlock(CurrentGame.CurrentBlock);
        blockManager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        DrawNextBlock(CurrentGame.Holder);
        CurrentGame.BlockPlaced = false;
    }

    private void DrawHeldBlock(Block heldBlock)
    {
        HoldImage.sprite = heldBlock != null ? BlockImages[heldBlock.Id - 1] : null;
    }

    private void DrawNextBlock(BlockHolder holder)
    {
        NextImage.sprite = BlockImages[holder.NextBlock.Id - 1];
    }

    private void ClearFullLayers()
    {
        int clearedLayers = 0;
        for (int y = CurrentGame.Grid.Y - 1; y > 0; y--)
        {
            if (CurrentGame.Grid.IsLayerFull(y))
            {
                ClearBlocksInRow(y);
                clearedLayers++;
                linesCleaned++;
            }
            else if (clearedLayers > 0)
            {
                MoveBlocksDown(y, clearedLayers);
            }
        }

        if (clearedLayers > 0)
        {
            CheckLevelUp();
            Manager.DrawLinesCompletedUI(score, level, clearedLayers,  DoubleScore);
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

    public void NextWithoutPlacing()
    {
        blockManager.ClearCurrentBlocks();
        CurrentGame.NextBlock();
        blockManager.CreateNewBlock(CurrentGame.CurrentBlock);
        blockManager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        DrawNextBlock(CurrentGame.Holder);
    }

    //We cant remove elements from a collection while iterating over it, so we first load tiles into the tilesToRemove List.
    public void ClearBlocksInRow(int y)
    {
        CurrentGame.Grid.ClearLayer(y);
        var tilesToRemove = new List<GameObject>();

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.y == YMax - 1 - y + 0.5f)
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
        CurrentGame.Grid.ClearColumn(x, z);
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
    public void DropAndRestart()
    {
        CurrentGame.DropBlock();
        RestartGameCycle();
    }

    private void MoveBlocksDown(int y, int drop)
    {
        CurrentGame.Grid.MoveLayerDown(y, drop);
        Vector3 dropVector = new Vector3(0, drop, 0);

        foreach (var tile in PlacedBlocks)
        {
            if (tile.transform.position.y == YMax - 1 - y + 0.5f)
            {
                tile.transform.position -= dropVector;
            }
        }
    }

    private void HandleKeyInputs()
    {
        foreach (var keyAction in keyActions)
        {
            if ((Input.GetKey(keyAction.Key) && IsDesiredHeld(keyAction.Key)) || Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value();
            }
        }

        blockManager.UpdateBlock(CurrentGame.CurrentBlock);
        blockManager.UpdatePrediction(CurrentGame.CurrentBlock);
    }

    private bool IsDesiredHeld(KeyCode key) =>
        key == GetKeyFromIndex(9) || key == GetKeyFromIndex(12) || key == GetKeyFromIndex(13);

    private void RotateCamera(float angle)
    {
        Vector3 direction = GameCamera.transform.position - lookPoint;
        direction = Quaternion.Euler(0, angle, 0) * direction;
        GameCamera.transform.position = lookPoint + direction;
        GameCamera.transform.LookAt(lookPoint);
    }

    public bool IsGameActive() =>
        !CurrentGame.GameOver && !CountdownText.gameObject.activeSelf && !Manager.GameMenu.IsPaused;

    private void UpdateFallDelay()
    {
        delay.AdjustDelay(score.CurrentScore, DelayMultiplier);
        timeSinceLastFall += Time.deltaTime;
    }
    public void SaveCurrentSettings()
    {
        GameSettings settings = new GameSettings
        {
            KeyBindings = new KeyCode[keyActions.Keys.Count]
        };

        keyActions.Keys.CopyTo(settings.KeyBindings, 0);

        SettingsManager.SaveSettings(settings);
    }

    public void LoadSettings()
    {
        GameSettings settings = SettingsManager.LoadSettings();
        KeyCode[] loadedKeys = settings.KeyBindings;
        InitializeKeyMappings(loadedKeys);
    }

    private void InitializeKeyMappings(KeyCode[] loadedBindings)
    {
        if (loadedBindings != null && loadedBindings.Length == keyActions.Count)
        {
            var actions = new List<Action>(keyActions.Values);
            keyActions.Clear();
            for (int i = 0; i < loadedBindings.Length; i++)
            {
                keyActions.Add(loadedBindings[i], actions[i]);
            }
        }
        else SetKeyMappingDefault();
    }

    private void SetKeyMappingDefault()
    {
        keyActions = new Dictionary<KeyCode, Action>
        {
            { KeyCode.UpArrow, () => { if(!LimitedMovement) {CurrentGame.XBack(); } } },
            { KeyCode.DownArrow, () => { if(!LimitedMovement) {CurrentGame.XForward(); } } },
            { KeyCode.LeftArrow, () => { if(!LimitedMovement) {CurrentGame.ZBack(); } } },
            { KeyCode.RightArrow, () => { if(!LimitedMovement) {CurrentGame.ZForward(); } } },
            { KeyCode.Q, () => CurrentGame.RotateBlockCCW() },
            { KeyCode.E, () => CurrentGame.RotateBlockCW() },
            { KeyCode.A, () => CurrentGame.SwitchToDifAxis(0) },
            { KeyCode.S, () => CurrentGame.SwitchToDifAxis(1) },
            { KeyCode.D, () => CurrentGame.SwitchToDifAxis(2) },
            { KeyCode.LeftShift, () => {if(!Freezed) {AdjustScoreAndDelay(); } } },
            { KeyCode.Space, () => DropAndRestart() },
            { KeyCode.C, () => HoldAndDrawBlocks() },
            { KeyCode.K, () => RotateCamera(RotationAngle) },
            { KeyCode.L, () => RotateCamera(-RotationAngle) },
            { KeyCode.R, () => BackgroundRenderer.ResetToDefault() },
            { KeyCode.Escape, () => Manager.Pause() }
        };
    }

    public void EnqueueAction(Action action)
    {
        if (action != null)
        {
            actionQueue.Enqueue(action);
        }
    }

    private void ExecuteQueuedActions()
    {
        while (actionQueue.Count > 0)
        {
            actionQueue.Dequeue().Invoke();
            Debug.Log("action invoked");
        }
    }

    private void AdjustScoreAndDelay()
    {
        delay.CurrentDelay = 75;
        score.IncrementScore(DoubleScore);
        Manager.DrawScoreUI(score.CurrentScore);
    }

    private void HoldAndDrawBlocks()
    {
        blockManager.HoldCurrentBlock();
        DrawHeldBlock(CurrentGame.HeldBlock);
        DrawNextBlock(CurrentGame.Holder);
    }

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
        CountdownPanel.SetActive(false);
    }
}
