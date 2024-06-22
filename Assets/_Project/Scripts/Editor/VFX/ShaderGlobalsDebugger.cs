using DualityGame.VFX;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.VFX
{
    public class ShaderGlobalsDebugger : EditorWindow
    {
        [MenuItem("Duality Game/Shader Globals Debugger")]
        private static void ShowWindow()
        {
            var window = GetWindow<ShaderGlobalsDebugger>();
            window.titleContent = new GUIContent("Shader Globals Debugger");
            window.Show();
        }

        private Toggle _warpEffectEnabledToggle;
        private Vector3Field _warpCenterField;
        private FloatField _warpRadiusField;
        private IntegerField _currentRealmField;
        private IntegerField _warpToRealmField;
        private FloatField _warpTransitionField;
        private Vector3Field _playerPositionField;


        private void CreateGUI()
        {
            rootVisualElement.style.paddingBottom = 5f;
            rootVisualElement.style.paddingLeft = 5f;
            rootVisualElement.style.paddingRight = 5f;
            rootVisualElement.style.paddingTop = 5f;

            _warpEffectEnabledToggle = new Toggle
            {
                label = "Warp Effect Enabled",
                value = ShaderGlobals.WarpEffectEnabled,
            };
            _warpCenterField = new Vector3Field
            {
                label = "Warp Center",
                value = ShaderGlobals.WarpCenter,
            };
            _warpRadiusField = new FloatField
            {
                label = "Warp Radius",
                value = ShaderGlobals.WarpRadius,
            };
            _currentRealmField = new IntegerField
            {
                label = "Current Realm",
                value = ShaderGlobals.CurrentRealm,
            };
            _warpToRealmField = new IntegerField
            {
                label = "Warp To Realm",
                value = ShaderGlobals.WarpToRealm,
            };
            _warpTransitionField = new FloatField
            {
                label = "Warp Transition",
                value = ShaderGlobals.WarpTransition,
            };
            _playerPositionField = new Vector3Field
            {
                label = "Player Position",
                value = ShaderGlobals.PlayerPosition,
            };

            rootVisualElement.Add(_warpEffectEnabledToggle);
            rootVisualElement.Add(_warpCenterField);
            rootVisualElement.Add(_warpRadiusField);
            rootVisualElement.Add(_currentRealmField);
            rootVisualElement.Add(_warpToRealmField);
            rootVisualElement.Add(_warpTransitionField);
            rootVisualElement.Add(_playerPositionField);
            rootVisualElement.SetEnabled(false);
        }

        private void Update()
        {
            _warpEffectEnabledToggle.value = ShaderGlobals.WarpEffectEnabled;
            _warpCenterField.value = ShaderGlobals.WarpCenter;
            _warpRadiusField.value = ShaderGlobals.WarpRadius;
            _currentRealmField.value = ShaderGlobals.CurrentRealm;
            _warpToRealmField.value = ShaderGlobals.WarpToRealm;
            _warpTransitionField.value = ShaderGlobals.WarpTransition;
            _playerPositionField.value = ShaderGlobals.PlayerPosition;
        }
    }
}
