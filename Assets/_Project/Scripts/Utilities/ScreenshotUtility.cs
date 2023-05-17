using System;
using System.IO;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Utilities
{
    public class ScreenshotUtility : MonoBehaviour
    {
        [SerializeField] private InputActionReference _hotkey;
        [SerializeField] private string _screenshotDirectory = "Screenshots";
       
        private void Awake()
        {
            if (!Directory.Exists(_screenshotDirectory))
            {
                Directory.CreateDirectory(_screenshotDirectory);
            }

            _hotkey.action.performed += TriggerScreenshot;
            _hotkey.action.Enable();
        }

        private void OnDestroy() => _hotkey.action.performed -= TriggerScreenshot;

        private void TriggerScreenshot(InputAction.CallbackContext obj)
        {
            var filename = $"{DateTime.Now:yyyyMMddHHmmss}.png";
            if (SceneManager.CurrentSceneGroup.Value != null)
            {
                filename = $"{SceneManager.CurrentSceneGroup.Value.name}-{filename}";
            }

            ScreenCapture.CaptureScreenshot(Path.Combine(_screenshotDirectory, filename));
            Debug.Log($"Saving screenshot to {filename}");
        }
    }
}
