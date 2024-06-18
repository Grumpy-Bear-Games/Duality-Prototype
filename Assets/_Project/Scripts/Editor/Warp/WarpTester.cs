using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using DualityGame.VFX;
using Unity.Properties;

namespace DualityGame.Editor.Warp
{
    public class WarpTester : EditorWindow
    {
        [MenuItem("Duality Game/Warp Tester")]
        private static void ShowWindow()
        {
            var window = GetWindow<WarpTester>();
            window.titleContent = new GUIContent("Warp Tester");
            window.Show();
        }

        private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

        private void OnPlayModeStateChanged(PlayModeStateChange playModeState)
        {
            switch (playModeState)
            {
                case PlayModeStateChange.ExitingEditMode:
                    rootVisualElement.SetEnabled(false);
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    rootVisualElement.SetEnabled(true);
                    UpdateWarp();
                    break;
            }
        }

        private void OnDestroy() => ShaderGlobals.Reset();

        private Toggle _warpEffectEnabled;
        private Slider _radiusSlider;
        private ObjectField _currentRealm;
        private ObjectField _warpToRealm;
        private Vector3Field _playerPositionField;

        private void CreateGUI()
        {
            rootVisualElement.style.paddingBottom = 5f;
            rootVisualElement.style.paddingLeft = 5f;
            rootVisualElement.style.paddingRight = 5f;
            rootVisualElement.style.paddingTop = 5f;
            rootVisualElement.dataSource = WarpTestConfig.instance;

            _warpEffectEnabled = new Toggle
            {
                label = "Warp Effect Enabled",
                value = false,
            };
            _warpEffectEnabled.RegisterValueChangedCallback(_ => UpdateWarp());

            var binding = new DataBinding
            {
                bindingMode = BindingMode.TwoWay,
            };
            _radiusSlider = new Slider(0f, 150f, SliderDirection.Horizontal)
            {
                label = "Radius",
                dataSourcePath = new PropertyPath(nameof(WarpTestConfig.instance.Radius)),
            };
            _radiusSlider.SetBinding("value", binding);
            _radiusSlider.RegisterValueChangedCallback(_ => UpdateWarp());

            _currentRealm = new ObjectField
            {
                label = "Current realm",
                objectType = typeof(DualityGame.Realm.Realm),
                dataSourcePath = new PropertyPath(nameof(WarpTestConfig.instance.CurrentRealm)),
            };
            _currentRealm.SetBinding("value", binding);
            _currentRealm.RegisterValueChangedCallback(_ => UpdateWarp());

            _warpToRealm = new ObjectField
            {
                label = "Warp to realm",
                objectType = typeof(DualityGame.Realm.Realm),
                dataSourcePath = new PropertyPath(nameof(WarpTestConfig.instance.WarpToRealm)),
            };
            _warpToRealm.SetBinding("value", binding);
            _warpToRealm.RegisterValueChangedCallback(_ => UpdateWarp());

            _playerPositionField = new Vector3Field
            {
                label = "Player Position",
                dataSourcePath = new PropertyPath(nameof(WarpTestConfig.instance.PlayerPosition)),
            };
            _playerPositionField.SetBinding("value", binding);
            _playerPositionField.RegisterValueChangedCallback(_ => UpdateWarp());

            rootVisualElement.Add(_warpEffectEnabled);
            rootVisualElement.Add(_radiusSlider);
            rootVisualElement.Add(_currentRealm);
            rootVisualElement.Add(_warpToRealm);
            rootVisualElement.Add(_playerPositionField);

            rootVisualElement.SetEnabled(!EditorApplication.isPlaying);
            if (!EditorApplication.isPlaying) UpdateWarp();
        }

        private void UpdateWarp()
        {
            ShaderGlobals.PlayerPosition = WarpTestConfig.instance.PlayerPosition;
            ShaderGlobals.WarpCenter = WarpTestConfig.instance.PlayerPosition;
            ShaderGlobals.WarpRadius = WarpTestConfig.instance.Radius;
            ShaderGlobals.WarpEffectEnabled = _warpEffectEnabled.value;
            ShaderGlobals.CurrentRealm = WarpTestConfig.instance.CurrentRealm != null ? WarpTestConfig.instance.CurrentRealm.LevelLayer : 0;
            ShaderGlobals.WarpToRealm = WarpTestConfig.instance.WarpToRealm != null ? WarpTestConfig.instance.WarpToRealm.LevelLayer : 1;
        }
    }
}
