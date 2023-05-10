using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
	public class PlayerInputs : MonoBehaviour
	{
		[SerializeField] private InputActionReference _moveAction;
		[SerializeField] private InputActionReference _jumpAction;
		[SerializeField] private InputActionReference _sprintAction;
		
		private void Awake()
		{
			_moveAction.action.performed += context => Move = context.ReadValue<Vector2>();
			_moveAction.action.canceled += _ => Move = Vector2.zero;
			
			_sprintAction.action.performed += _ => Sprint = true;
			_sprintAction.action.canceled += _ => Sprint = false;
		}

		private void OnEnable()
		{
			_moveAction.action.Enable();
			_jumpAction.action.Enable();
			_sprintAction.action.Enable();
		}

		private void OnDisable()
		{
			_moveAction.action.Disable();
			_jumpAction.action.Disable();
			_sprintAction.action.Disable();
		}

		public Vector2 Move { get; private set; } = Vector2.zero;
		public bool Jump => _jumpAction.action.triggered;
		public bool Sprint { get; private set; }
	}
	
}
