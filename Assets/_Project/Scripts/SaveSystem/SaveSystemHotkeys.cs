using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.SaveSystem
{
    public class SaveSystemHotkeys : MonoBehaviour
    {
        #region Hotkeys - Just for testing
        [SerializeField] private InputActionReference _saveKey;
        [SerializeField] private InputActionReference _loadKey;
        [SerializeField] private InputActionReference _clearKey;

        [SerializeField] private GameSession _gameSession;
        
        private void OnEnable()
        {
            _saveKey.action.performed += OnSave;
            _loadKey.action.performed += OnLoad;
            _clearKey.action.performed += OnClear;

            _saveKey.action.Enable();
            _loadKey.action.Enable();
            _clearKey.action.Enable();
        }

        private void OnDisable()
        {
            _saveKey.action.Disable();
            _loadKey.action.Disable();
            _clearKey.action.Disable();
            
            _saveKey.action.performed -= OnSave;
            _loadKey.action.performed -= OnLoad;
            _clearKey.action.performed -= OnClear;
            
        }

        private void OnClear(InputAction.CallbackContext obj)
        {
            Debug.Log("Clearing state");
            CoroutineRunner.Run(_gameSession.NewGame());
        }

        private void OnLoad(InputAction.CallbackContext obj)
        {
            Debug.Log("Loading state");
            CoroutineRunner.Run(_gameSession.LoadGame());
        }

        private void OnSave(InputAction.CallbackContext obj)
        {
            Debug.Log("Saving state");
            _gameSession.SaveGame();
        }

        #endregion
    }
}
