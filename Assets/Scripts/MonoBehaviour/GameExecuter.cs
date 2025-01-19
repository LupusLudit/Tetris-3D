using Assets.Scripts;
using Assets.Scripts.Logic;
using Assets.Scripts.MonoBehaviour;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameExecuter : MonoBehaviour
{

    [Header("UI Elements")]
    public UIManager UI;
    public KeyBinding KeyBinding;
    public StartingCountdown Countdown;
    public ImageDrawer ImageDrawer;

    [Header("Game Settings")]
    public Camera GameCamera;
    public BackgroundRenderer BackgroundRenderer;
    public int XMax, YMax, ZMax;
    public GameObject[] BlockPrefabs;

    [Header("Other")]
    public SoundEffects SoundEffects;

    public Game CurrentGame { get; private set; }
    public double DelayMultiplier { get; set; } = 1;
    public Score Score;

    public GameManager Manager;
    private Queue<Action> actionQueue = new Queue<Action>();
    private Vector3 lookPoint;
    private DelayManager delay;
    private float timeSinceLastFall;
    private int level = 0;
    private int linesCleaned = 0;
    private const float RotationAngle = 0.6f;

    public KeyCode[] Keys;
    private Action[] actions;

    void Start()
    {
        CurrentGame = new Game(XMax, YMax, ZMax);
        Score = new Score();
        Manager = new GameManager();
        Manager.Initialize(this);
        lookPoint = BoardDimensions.calcLookPoint(new Vector3(XMax, YMax, ZMax), GameCamera); 

        delay = new DelayManager(750, 50, 25);

        SetKeyMappingDefault();
        InitializeActions();
        LoadSettings();
        StartCoroutine(Countdown.StartCounting());

        Manager.CreateNewBlock(CurrentGame.CurrentBlock);
        Manager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        ImageDrawer.DrawNextBlock(CurrentGame.Holder);

        KeyBinding.InitializeButtonLabels(Keys);
        KeyBinding.InitializeHintLabels(Keys); 
        timeSinceLastFall = 0f;
    }

    void Update()
    {
        if (!IsGameActive())
        {
            if(CurrentGame.GameOver) UI.DrawGameOverScreen();
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
        if(!Manager.Freezed) CurrentGame.MoveBlockDown();
        if (CurrentGame.BlockPlaced) RestartGameCycle();

        Manager.UpdateBlock(CurrentGame.CurrentBlock);
        Manager.UpdatePrediction(CurrentGame.CurrentBlock);
    }

    private void RestartGameCycle()
    {
        Manager.PlaceCurrentBlock();
        SoundEffects.PlayEffect(2); //placing sound effect
        Manager.ClearFullLayers();
        CurrentGame.NextBlock();
        Manager.CreateNewBlock(CurrentGame.CurrentBlock);
        Manager.CreateBlockPrediction(CurrentGame.CurrentBlock);
        ImageDrawer.DrawNextBlock(CurrentGame.Holder);
        CurrentGame.BlockPlaced = false;
    }

    public void DropAndRestart()
    {
        CurrentGame.DropBlock();
        RestartGameCycle();
    }


    private void HandleKeyInputs()
    {
        for (int i = 0; i < Keys.Length; i++)
        {
            if ((Input.GetKey(Keys[i]) && IsDesiredHeld(Keys[i])) || Input.GetKeyDown(Keys[i]))
            {
                actions[i].Invoke();
            }
        }

        Manager.UpdateBlock(CurrentGame.CurrentBlock);
        Manager.UpdatePrediction(CurrentGame.CurrentBlock);
    }

    private bool IsDesiredHeld(KeyCode key) =>
        key == Keys[9] || key == Keys[12] || key == Keys[13];

    private void RotateCamera(float angle)
    {
        Vector3 direction = GameCamera.transform.position - lookPoint;
        direction = Quaternion.Euler(0, angle, 0) * direction;
        GameCamera.transform.position = lookPoint + direction;
        GameCamera.transform.LookAt(lookPoint);
    }

    public bool IsGameActive() =>
        !CurrentGame.GameOver && !Countdown.CountdownText.gameObject.activeSelf && !UI.GameMenu.IsPaused;

    private void UpdateFallDelay()
    {
        delay.AdjustDelay(Score.CurrentScore, DelayMultiplier);
        timeSinceLastFall += Time.deltaTime;
    }
    public void SaveCurrentSettings()
    {
        GameSettings settings = new GameSettings
        {
            KeyBindings = new KeyCode[Keys.Length]
        };

        for (int i = 0; i < Keys.Length; i++)
        {
            settings.KeyBindings[i] = Keys[i];
        }

        FileManager.SaveToFile(settings);
    }

    public void LoadSettings()
    {
        GameSettings settings = FileManager.LoadSettings();
        KeyCode[] loadedKeys = settings.KeyBindings;
        InitializeKeyMappings(loadedKeys);
    }

    private void InitializeKeyMappings(KeyCode[] loadedBindings)
    {
        if (loadedBindings != null && loadedBindings.Length == Keys.Length)
        {
            for (int i = 0; i < loadedBindings.Length; i++)
            {
                Keys[i] = loadedBindings[i];
            }
        }
        else SetKeyMappingDefault();
    }

    private void SetKeyMappingDefault()
    {
        Keys = new KeyCode[]{
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.Q,
            KeyCode.E,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.LeftShift,
            KeyCode.Space,
            KeyCode.C,
            KeyCode.K,
            KeyCode.L,
            KeyCode.R,
            KeyCode.Escape
        };
    }

    private void InitializeActions()
    {
        actions = new Action[]
        {
            () => { if (!Manager.LimitedMovement) { CurrentGame.XBack(); SoundEffects.PlayEffect(1); } },
            () => { if (!Manager.LimitedMovement) { CurrentGame.XForward(); SoundEffects.PlayEffect(1); } },
            () => { if (!Manager.LimitedMovement) { CurrentGame.ZBack(); SoundEffects.PlayEffect(1);  } },
            () => { if (!Manager.LimitedMovement) { CurrentGame.ZForward(); SoundEffects.PlayEffect(1);  } },
            () => { CurrentGame.RotateBlockCCW(); SoundEffects.PlayEffect(1); },
            () => { CurrentGame.RotateBlockCW(); SoundEffects.PlayEffect(1); },
            () => {
                if(CurrentGame.CurrentBlock.CurrentState != 0)
                {
                    CurrentGame.SwitchToDifAxis(0);
                    SoundEffects.PlayEffect(1);
                }
            },
            () => {
                if(CurrentGame.CurrentBlock.CurrentState != 1)
                {
                    CurrentGame.SwitchToDifAxis(1);
                    SoundEffects.PlayEffect(1);
                }
            },
            () => {
                if(CurrentGame.CurrentBlock.CurrentState != 2)
                {
                    CurrentGame.SwitchToDifAxis(2);
                    SoundEffects.PlayEffect(1);
                }
            },
            () => { if (!Manager.Freezed) { AdjustScoreAndDelay(); } },
            () => DropAndRestart(),
            () => HoldAndDrawBlocks(),
            () => RotateCamera(RotationAngle),
            () => RotateCamera(-RotationAngle),
            () => BackgroundRenderer.ResetToDefault(),
            () => UI.Pause()
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
        }
    }

    private void AdjustScoreAndDelay()
    {
        delay.CurrentDelay = 75;
        Score.IncrementScore(Manager.DoubleScore);
        UI.DrawScoreUI(Score.CurrentScore);
    }

    private void HoldAndDrawBlocks()
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
