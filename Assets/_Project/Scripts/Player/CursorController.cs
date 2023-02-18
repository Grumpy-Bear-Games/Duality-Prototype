using System;
using UnityEngine;

namespace DualityGame.Player
{
    public class CursorController : MonoBehaviour
    {
        #if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")]
        [SerializeField] private bool _confined = true;

        private bool _hidden;
        private bool _hasFocus = true;

        private void Awake() => UpdateCursor();

        public void HideCursor(bool hidden)
        {
            _hidden = hidden;
            UpdateCursor();
        }

        private void UpdateCursor()
        {
            if (_hasFocus)
            {
                if (_hidden)
                    Cursor.lockState = CursorLockMode.Locked;
                else
                    Cursor.lockState = _confined ? CursorLockMode.Confined : CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }


        private void OnApplicationFocus(bool hasFocus)
        {
            _hasFocus = hasFocus;
            UpdateCursor();
        }
        #endif
    }
}
