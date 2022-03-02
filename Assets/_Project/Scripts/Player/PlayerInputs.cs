using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
	public class PlayerInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 _move;

		public bool _jump;
		public bool _sprint;

		[Header("Movement Settings")]
		public bool _analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool _cursorLocked = true;
#endif

		public void OnMove(InputValue value) => _move = value.Get<Vector2>();

		public void OnJump(InputValue value) => _jump = value.isPressed;

		public void OnSprint(InputValue value) => _sprint = value.isPressed;

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus) => SetCursorState(_cursorLocked);

		private static void SetCursorState(bool newState) => Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;

#endif

	}
	
}
