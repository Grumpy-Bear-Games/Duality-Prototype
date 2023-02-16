using UnityEngine;

namespace DualityGame.Player
{
    // TODO: To be fleshed out later
    public class CursorController : MonoBehaviour
    {
        #if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")]
        public bool _cursorLocked = true;
        private void OnApplicationFocus(bool hasFocus) => SetCursorState(_cursorLocked);
        private static void SetCursorState(bool newState) => Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        #endif
    }
}
