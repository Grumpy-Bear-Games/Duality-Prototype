using System;
using DualityGame.Core;
using DualityGame.SaveSystem;
using DualityGame.VFX;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseMenu: MonoBehaviour
    {
        [SerializeField] private UnityEvent _onContinue;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private SceneGroup _mainMenuSceneGroup;
        [SerializeField] private ScreenFader _screenFader;

        private VisualElement _frame;

        private SettingsMenu _settingsMenu;
        private void Awake()
        {
            _settingsMenu = GetComponent<SettingsMenu>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _frame = root.Q<VisualElement>("PauseMenu");

            _settingsMenu.OnHide += () =>
            {
                Show();
                FocusHelper.Pop();
            };

            _frame.Q<Button>("ContinueButton").clicked += Continue;
            _frame.Q<Button>("SettingsButton").clicked += Settings;
            _frame.Q<Button>("ExitToMainMenuButton").clicked += ExitToMainMenu;

            var confirmationDialog = root.Q<ConfirmationDialog>();
            confirmationDialog.Hide();
            confirmationDialog.OnConfirm += ExitGame;

            _frame.Q<Button>("QuitButton").clicked += confirmationDialog.Show;
            _frame.RegisterCallback<NavigationCancelEvent>( _ => Continue());

            Hide();
        }

        private void Continue()
        {
            Hide();
            _onContinue.Invoke();
        }

        private void Settings()
        {
            FocusHelper.Push();
            Hide();
            _settingsMenu.Show();
        }

        private void ExitToMainMenu()
        {
            _gameSession.SaveGame();
            CoroutineRunner.Run(_screenFader.Wrap(_mainMenuSceneGroup.Load_CO()));
        }

        private void Hide() => _frame.AddToClassList("Hide");

        private void Show()
        {
            _frame.RemoveFromClassList("Hide");
            _frame.Q<Button>("ContinueButton").Focus();
        }

        private void ExitGame()
        {
            _gameSession.SaveGame();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }

        private void OnEnable() => Show();

        private void OnDisable()
        {
            _settingsMenu.Hide();
            Hide();
        }
    }
}
