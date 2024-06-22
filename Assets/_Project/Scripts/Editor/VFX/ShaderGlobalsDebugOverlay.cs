using DualityGame.VFX;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.VFX
{
    [Overlay(typeof(SceneView), "Shader Globals Debugger")]
    public class ShaderGlobalsDebugOverlay : Overlay
    {
        private Toggle _warpEffectEnabledToggle;
        private Vector3Field _warpCenterField;
        private FloatField _warpRadiusField;
        private IntegerField _currentRealmField;
        private IntegerField _warpToRealmField;
        private FloatField _warpTransitionField;
        private Vector3Field _playerPositionField;


        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            root.Add(_warpEffectEnabledToggle = new Toggle
            {
                label = "Warp Effect Enabled",
                value = ShaderGlobals.WarpEffectEnabled,
            });
            root.Add(_warpCenterField = new Vector3Field
            {
                label = "Warp Center",
                value = ShaderGlobals.WarpCenter,
            });
            root.Add(_warpRadiusField = new FloatField
            {
                label = "Warp Radius",
                value = ShaderGlobals.WarpRadius,
            });
            root.Add(_currentRealmField = new IntegerField
            {
                label = "Current Realm",
                value = ShaderGlobals.CurrentRealm,
            });
            root.Add(_warpToRealmField = new IntegerField
            {
                label = "Warp To Realm",
                value = ShaderGlobals.WarpToRealm,
            });
            root.Add(_warpTransitionField = new FloatField
            {
                label = "Warp Transition",
                value = ShaderGlobals.WarpTransition,
            });
            root.Add(_playerPositionField = new Vector3Field
            {
                label = "Player Position",
                value = ShaderGlobals.PlayerPosition,
            });

            root.SetEnabled(false);
            return root;
        }

        public override void OnCreated() => displayedChanged += OnDisplayedChanged;
        public override void OnWillBeDestroyed() => displayedChanged -= OnDisplayedChanged;

        private void OnDisplayedChanged(bool _)
        {
            Debug.Log($"ShaderGlobalsDebugOverlay.OnDisplayedChanged: {displayed}");
            if (displayed)
            {
                EditorApplication.update += Update;
            }
            else
            {
                EditorApplication.update -= Update;
            }
        }

        private void Update()
        {
            if (_warpRadiusField == null) return;
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
