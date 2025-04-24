using Assets.Scripts.Events;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Audio;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic.Managers
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="KeyManager"]/*'/>
    public class KeyManager
    {
        private GameExecuter executer;
        private GameManager manager;
        private Game game;
        private SoundEffects soundEffects;
        private Action[] actions;

        public KeyCode[] Keys;
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

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            AdjustKeyEvents.OnRotation -= AdjustMovements;
            AdjustKeyEvents.OnReset -= InitializeActions;
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            SetKeyMappingDefault();
            InitializeActions();
            LoadSettings();
        }

        /// <summary>
        /// Handles the players key inputs.
        /// </summary>
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

        /// <summary>
        /// Saves the current settings to the json file.
        /// </summary>
        public void SaveCurrentSettings()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "keybinds.json");


            KeySettings settings = new KeySettings
            {
                KeyBinds = new KeyCode[Keys.Length]
            };

            for (int i = 0; i < Keys.Length; i++)
            {
                settings.KeyBinds[i] = Keys[i];
            }

            FileManager.SaveToFile(settings, filePath);
        }

        /// <summary>
        /// Adjusts the key settings for the movements (UpArrow, DownArrow, LeftArrow and RightArrow by default).
        /// </summary>
        /// <param name="movedRight">
        /// If set to <c>true</c> it means that the board had moved to right. Otherwise, <c>false</c>.
        /// The pattern is then picked accordingly.
        /// </param>
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

        /// <summary>
        /// Determines whether a desired key is being held.
        /// Explanation: We want some keys to be held down (like the movement keys) and not just pressed, this method checks if the key is one of those.
        /// </summary>
        /// <param name="key">The key that was pressed/is being held.</param>
        /// <returns>
        ///   <c>true</c> if the key is between those that need to be held. Otherwise, <c>false</c>.
        /// </returns>
        private bool IsDesiredHeld(KeyCode key) => key == Keys[9] || key == Keys[12] || key == Keys[13];

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "keybinds.json");

            KeySettings settings = FileManager.LoadFromFile<KeySettings>(filePath, "keybinds.json");
            if (settings.KeyBinds != null)
            {
                InitializeKeyMappings(settings.KeyBinds);
            }
            else
            {
                SetKeyMappingDefault();
                SaveCurrentSettings();
            }
        }

        /// <summary>
        /// Initializes the key mappings if it they were loaded correctly.
        /// If not, it sets the key mappings to default values.
        /// </summary>
        /// <param name="loadedBindings">The loaded key bindings.</param>
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

        /// <summary>
        /// Sets the key settings to default values.
        /// </summary>
        public void SetKeyMappingDefault()
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

        /// <summary>
        /// Initializes the actions for the key inputs.
        /// </summary>
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
