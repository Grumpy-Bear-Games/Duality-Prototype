using System;
using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Player
{
	[RequireComponent(typeof(SaveableEntity))]
	public class AnimationController : MonoBehaviour, ISaveableComponent
    {
        [SerializeField] private Animator _animator;
		[SerializeField] private PlayerInputs _input;
        [SerializeField] private CharacterController _controller;

	
        private enum Direction {
            Up, Down, Left, Right
        }

        private enum State {
            Idle, InAir, Run
        }


        private readonly int[] _animID = new int[12];
        private Direction _direction = Direction.Down;
        private State _state = State.Idle;
        
		private void Awake() => AssignAnimationIDs();

		private void Update()
		{
			UpdateDirectionFromMovement();
			UpdateStateFromMovement();
			UpdateAnimation();
		}


		private void AssignAnimationIDs()
		{
			foreach (int state in Enum.GetValues(typeof(State))) {
				var stateName = Enum.GetName(typeof(State), state);
				foreach (int direction in Enum.GetValues(typeof(Direction)))
				{
					var directionName = Enum.GetName(typeof(Direction), direction);
					_animID[state * 4 + direction] = Animator.StringToHash($"MC {stateName} {directionName}");
				}
			}
		}
		
		private void UpdateDirectionFromMovement()
		{
			if (_input.Move == Vector2.zero) return;
			if (Mathf.Approximately(_input.Move.x, 0f)) {
				_direction = (_input.Move.y > Mathf.Epsilon) ? Direction.Up : Direction.Down;
			} else {
				_direction = (_input.Move.x < Mathf.Epsilon) ? Direction.Left : Direction.Right;
			}
		}
		
		private void UpdateStateFromMovement()
		{
			if (!_controller.isGrounded) {
				_state = State.InAir;
			} else {
				_state = (_input.Move == Vector2.zero) ? State.Idle : State.Run;
			}
		}

		private void UpdateAnimation()
		{
			var currentAnimation = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			var updatedAnimation = _animID[(int)_state * 4 + (int)_direction];
			if (currentAnimation != updatedAnimation) _animator.Play(updatedAnimation);
		}
        
		private void Reset()
		{
			_animator = GetComponentInChildren<Animator>();
			_input = GetComponentInParent<PlayerInputs>();
			_controller = GetComponentInParent<CharacterController>();
		}

		object ISaveableComponent.CaptureState()
		{
			return (int)_direction;
		}

		void ISaveableComponent.RestoreState(object state)
		{
			_direction = (Direction)state;
		}
    }
}
