using UnityEngine;

[RequireComponent(typeof(CharacterMovementHandler))]
public class CharacterAnimationHandler : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	private Animator _animator;
	private CharacterMovementHandler _movementHandler;

    private static readonly int s_isJump = Animator.StringToHash("isJump");
    private static readonly int s_isMove = Animator.StringToHash("isMove");
    private static readonly int s_AirVelocityY = Animator.StringToHash("AirVelocityY");

    private bool _isMoving;

    public void Init(SpriteRenderer sprite, CharacterMovementHandler movementHandler)
	{
		_spriteRenderer = sprite;
		_animator = sprite.GetComponent<Animator>();
		_movementHandler = movementHandler;

        Subscribe();
	}

    private void Subscribe()
    {
		_movementHandler.ActionJump += OnJump;
		_movementHandler.ActionLanded += OnLanded;
        _movementHandler.ActionKnockback += OnKnockback;
	}

    private void OnDisable()
    {
        _movementHandler.ActionJump -= OnJump;
        _movementHandler.ActionLanded -= OnLanded;
        _movementHandler.ActionKnockback -= OnKnockback;
    }

    private void Update()
    {
        HandleMovementAnimation();
        HandleAirborneAnimation();
    }

    private void HandleMovementAnimation()
    {
        if (_movementHandler.IsFreeAndReady)
        {
            float moveDirection = _movementHandler.InputDirection;
            bool isMovingNow = Mathf.Abs(moveDirection) > 0.01f;

            if (isMovingNow != _isMoving)
            {
                _isMoving = isMovingNow;
                _animator.SetBool(s_isMove, _isMoving);
            }

            if (moveDirection > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (moveDirection < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (_isMoving)
            {
                _isMoving = false;
                _animator.SetBool(s_isMove, false);
            }
        }
    }

    private void HandleAirborneAnimation()
    {
        if (!_movementHandler.IsOnGround)
        {
            _animator.SetFloat(s_AirVelocityY, _movementHandler.Rigidbody.velocity.y);
        }
    }

    private void OnJump()
	{
		_animator.SetBool(s_isJump, true);
	}

	private void OnLanded()
	{
        _animator.SetBool(s_isJump, false);
        _animator.SetFloat(s_AirVelocityY, 0);
    }

    private void OnKnockback()
    {
        _animator.SetBool(s_isJump, false);
		_animator.SetBool(s_isMove, false);
	}
}
