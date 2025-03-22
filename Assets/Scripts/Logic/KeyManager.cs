using Assets.Scripts.Events;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class KeyManager
    {
        private GameExecuter executer;
        private GameManager manager;
        private Game game;
        private SoundEffects soundEffects;
        public KeyCode[] Keys;
        private Action[] actions;

        public KeyManager(GameExecuter gameExecuter)
        {
            executer = gameExecuter;
            manager = gameExecuter.Manager;
            game = gameExecuter.CurrentGame;
            soundEffects = gameExecuter.SoundEffects;

            Initialize();

            AdjustKeyEvents.OnRotation += AdjustMovements;
            AdjustKeyEvents.OnReset += InitializeActions;
        }

        public void Dispose()
        {
            AdjustKeyEvents.OnRotation -= AdjustMovements;
            AdjustKeyEvents.OnReset -= InitializeActions;
        }


        private void Initialize()
        {
            SetKeyMappingDefault();
            InitializeActions();
            LoadSettings();
        }

        public void HandleKeyInputs()
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                if ((Input.GetKey(Keys[i]) && IsDesiredHeld(Keys[i])) || Input.GetKeyDown(Keys[i]))
                {
                    actions[i].Invoke();
                }
            }

            if (executer.IsGameActive())
            {
                manager.UpdateBlock(game.CurrentBlock);
                manager.UpdatePrediction(game.CurrentBlock);
            }
        }

        public void SaveCurrentSettings()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "settings.json");

            GameSettings settings = new GameSettings
            {
                KeyBinds = new KeyCode[Keys.Length]
            };

            for (int i = 0; i < Keys.Length; i++)
            {
                settings.KeyBinds[i] = Keys[i];
            }

            FileManager.SaveToFile(settings, filePath);
        }

        public void AdjustMovements(bool movedRight)
        {
            //Pattern for switching the movement keys to the right or left
            int[] pattern = movedRight
                ? new int[] { 2, 3, 1, 0 }
                : new int[] { 3, 2, 0, 1 };

            Action[] newActions = new Action[4];

            for (int i = 0; i < 4; i++)
            {
                newActions[i] = actions[pattern[i]];
            }

            for (int i = 0; i < 4; i++)
            {
                actions[i] = newActions[i];
            }
        }

        private bool IsDesiredHeld(KeyCode key) =>
            key == Keys[9] || key == Keys[12] || key == Keys[13];


        private void LoadSettings()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "settings.json");
            GameSettings settings = FileManager.LoadFromFile<GameSettings>(filePath);
            KeyCode[] loadedKeys = settings.KeyBinds;
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
            () => { if (!manager.LimitedMovement) { game.XBack(); soundEffects.PlayEffect(1); } },
            () => { if (!manager.LimitedMovement) { game.XForward(); soundEffects.PlayEffect(1); } },
            () => { if (!manager.LimitedMovement) { game.ZBack(); soundEffects.PlayEffect(1);  } },
            () => { if (!manager.LimitedMovement) { game.ZForward(); soundEffects.PlayEffect(1);  } },
            () => { game.RotateBlockCCW(); soundEffects.PlayEffect(1); },
            () => { game.RotateBlockCW(); soundEffects.PlayEffect(1); },
            () => {
                if(game.CurrentBlock.CurrentState != 0)
                {
                    game.SwitchToDifAxis(0);
                    soundEffects.PlayEffect(1);
                }
            },
            () => {
                if(game.CurrentBlock.CurrentState != 1)
                {
                    game.SwitchToDifAxis(1);
                    soundEffects.PlayEffect(1);
                }
            },
            () => {
                if(game.CurrentBlock.CurrentState != 2)
                {
                    game.SwitchToDifAxis(2);
                    soundEffects.PlayEffect(1);
                }
            },
            () => { if (!manager.Freezed) { executer.AdjustDelay(); } },
            () => {executer.DropAndRestart();},
            () => executer.HoldAndDrawBlocks(),
            () => manager.RotateCamera(75f),
            () => manager.RotateCamera(-75f),
            () => executer.BackgroundRenderer.ResetToDefault(),
            () => executer.UI.Pause()
            };
        }
    }
}
