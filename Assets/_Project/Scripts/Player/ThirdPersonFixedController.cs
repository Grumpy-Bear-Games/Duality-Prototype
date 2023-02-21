using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace DualityGame.Player
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class ThirdPersonFixedController : MonoBehaviour, ISaveableComponent
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;


		[Header("Events")]
		[SerializeField] private UnityEvent _onJump;
		[SerializeField] private UnityEvent<float> _onLand;

		// player
		private float _speed;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private CharacterController _controller;
		private PlayerInputs _input;
		private bool _groundedLastFrame;


		private void Awake()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<PlayerInputs>();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			_groundedLastFrame = _controller.isGrounded;
		}

		private void Update()
		{
			JumpAndGravity();
			Move();

			if (!_groundedLastFrame && _controller.isGrounded) _onLand.Invoke(_verticalVelocity);
			_groundedLastFrame = _controller.isGrounded;
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			var targetSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			var velocity = _controller.velocity;
			var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

			const float speedOffset = 0.1f;
			var inputMagnitude = _input._analogMovement ? _input.Move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			var inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (_controller.isGrounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					_onJump.Invoke();
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		object ISaveableComponent.CaptureState() => transform.position;

		void ISaveableComponent.RestoreState(object state) => transform.position = (Vector3)state;
	}
}
