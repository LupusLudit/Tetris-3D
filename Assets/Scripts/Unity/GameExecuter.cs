using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameExecuter : MonoBehaviour
{

    [Header("UI Elements")]
    public UIManager UI;
    public KeyBinding KeyBindingUI;
    public StartingCountdown Countdown;
    public ImageDrawer ImageDrawer;

    [Header("Game Settings")]
    public BackgroundRenderer BackgroundRenderer;
    public int XMax, YMax, ZMax;
    public GameObject[] BlockPrefabs;

    [Header("Other")]
    public SoundEffects SoundEffects;
    public Camera GameCamera;
    public float HalfTileSize = 0.5f; //0.5 is the default size of a block, but this number can be adjusted for testing purposes

    public Game CurrentGame { get; private set; }
    public double DelayMultiplier { get; set; } = 1;
    public Score Score { get; private set; }
    public GameManager Manager { get; private set; }
    public KeyManager KeyManager { get; private set; }
    public Vector3 LookPoint { get; private set; }
    public int BlocksPlaced { get; private set; } = 0;

    private Queue<Action> actionQueue = new Queue<Action>();
    private DelayManager delay;
    private float timeSinceLastFall;

    void Start()
    {
        CurrentGame = new Game(XMax, YMax, ZMax);
        Score = new Score();
        LookPoint = BoardDimensions.calcLookPoint(new Vector3(XMax, YMax, ZMax), GameCamera);

        Manager = new GameManager();
        Manager.Initialize(this);
        delay = new DelayManager(750, 50, 25);
        KeyManager = new KeyManager(this);

        StartCoroutine(Countdown.StartCounting(() => SoundEffects));

        Manager.CreateNewBlock(CurrentGame.CurrentBlock);
        Manager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        ImageDrawer.DrawNextBlock(CurrentGame.Holder);

        KeyBindingUI.InitializeButtonLabels(KeyManager.Keys);
        KeyBindingUI.InitializeHintLabels(KeyManager.Keys); 
        timeSinceLastFall = 0f;
    }

    void Update()
    {
        BlocksPlaced = 0;
        if (!IsGameActive())
        {
            if (CurrentGame.GameOver) UI.DrawGameOverScreen();
            return;
        }
        else
        {
            UpdateFallDelay();
            KeyManager.HandleKeyInputs();
            ExecuteQueuedActions();

            if (timeSinceLastFall >= delay.CurrentDelay / 1000f)
            {
                ExecuteGameStep();
                timeSinceLastFall = 0f;
            }
        }
    }

    private void ExecuteGameStep()
    { 
        if (!Manager.Freezed) CurrentGame.MoveBlockDown();
        if (CurrentGame.BlockPlaced) RestartGameCycle();
        if (IsGameActive())
        {
            Manager.UpdateBlock(CurrentGame.CurrentBlock);
            Manager.UpdatePrediction(CurrentGame.CurrentBlock);
        }
    }

    private void RestartGameCycle()
    {
        BlocksPlaced++;
        Manager.PlaceCurrentBlock();
        SoundEffects.PlayEffect(2); //placing sound effect
        Manager.ClearFullLayers();
        CurrentGame.NextBlock();
        if (IsGameActive())
        {
            Manager.CreateNewBlock(CurrentGame.CurrentBlock);
            Manager.CreateBlockPrediction(CurrentGame.CurrentBlock);
            ImageDrawer.DrawNextBlock(CurrentGame.Holder);
        }
        CurrentGame.BlockPlaced = false;
    }

    public bool IsGameActive() =>
        !CurrentGame.GameOver && !Countdown.CountdownText.gameObject.activeSelf && !UI.GameMenu.IsPaused;

    public void DropAndRestart()
    {
        CurrentGame.DropBlock();
        RestartGameCycle();
    }

    private void UpdateFallDelay()
    {
        delay.AdjustDelay(Score.CurrentScore, DelayMultiplier);
        timeSinceLastFall += Time.deltaTime;
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
        }
    }

    public void AdjustDelay()
    {
        delay.CurrentDelay = 75;
        Score.IncrementScore(Manager.DoubleScore);
        UI.DrawScoreUI(Score.CurrentScore);
    }

    public void HoldAndDrawBlocks()
    {
        if (!CurrentGame.CanHold)
        {
            SoundEffects.PlayEffect(3);
            return;
        }
        else
        {
            Manager.HoldCurrentBlock();
            ImageDrawer.DrawHeldBlock(CurrentGame.HeldBlock);
            ImageDrawer.DrawNextBlock(CurrentGame.Holder);
            SoundEffects.PlayEffect(4);
        }
    }
}
