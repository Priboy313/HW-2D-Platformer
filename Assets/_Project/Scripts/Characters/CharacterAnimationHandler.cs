using UnityEngine;

[RequireComponent(typeof(CharacterMovementHandler))]
public class CharacterAnimationHandler : MonoBehaviour
{
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;
	private CharacterMovementHandler _movementHandler;

    private static readonly int s_isJump = Animator.StringToHash("isJump");
    private static readonly int s_isMove = Animator.StringToHash("isMove");
    private static readonly int s_AirVelocityY = Animator.StringToHash("AirVelocityY");

    private bool _isMoving;

    private void Awake()
	{
		bool hasErrors = false;

		_animator = GetComponentInChildren<Animator>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		if (_animator == null)
		{
			Debug.LogError("Animator not set!");
			hasErrors = true;
		}

		if (_spriteRenderer == null)
		{
            Debug.LogError("SpriteRenderer not set!");
            hasErrors = true;
        }

		enabled = !hasErrors;
		_movementHandler = GetComponent<CharacterMovementHandler>();
	}

    private void OnEnable()
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
            bool isMovingNow = moveDirection != 0;

            if (isMovingNow != _isMoving)
            {
                _isMoving = isMovingNow;
                _animator.SetBool(s_isMove, _isMoving);
            }

            if (_isMoving)
            {
                _spriteRenderer.flipX = moveDirection < 0;
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
