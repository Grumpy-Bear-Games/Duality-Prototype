using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
	public class PlayerInputs : MonoBehaviour
	{
		[Header("Movement Settings")]
		public bool _analogMovement;

		public Vector2 Move
		{
			get => enabled ? _move : Vector2.zero;
			private set => _move = value;
		}

		public bool Jump
		{
			get => enabled && _jump;
			private set => _jump = value;
		}

		public bool Sprint
		{
			get => enabled && _sprint;
			private set => _sprint = value;
		}

		private Vector2 _move;
		private bool _jump;
		private bool _sprint;

		public void OnMove(InputValue value) => Move = value.Get<Vector2>();

		public void OnJump(InputValue value) => Jump = value.isPressed;

		public void OnSprint(InputValue value) => Sprint = value.isPressed;
	}
	
}
