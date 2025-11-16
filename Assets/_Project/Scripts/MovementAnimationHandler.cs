using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(MovementHandler))]
public class MovementAnimationHandler : MonoBehaviour
{
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;
	private MovementHandler _movementHandler;

    private static readonly int s_isJump = Animator.StringToHash("isJump");
    private static readonly int s_isMove = Animator.StringToHash("isMove");
    private static readonly int s_AirVelocityY = Animator.StringToHash("AirVelocityY");

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
		_movementHandler = GetComponent<MovementHandler>();
	}

    private void OnEnable()
    {
		_movementHandler.ActionJump += OnJump;
		_movementHandler.ActionLanded += Onlanded;
	}

    private void OnDisable()
    {
        _movementHandler.ActionJump -= OnJump;
        _movementHandler.ActionLanded -= Onlanded;
    }

    private void Update()
    {
        HandleMovementAnimation();
        HandleAirborneAnimation();
    }

    private void HandleMovementAnimation()
    {
        float moveDirection = _movementHandler.InputDirection;
        bool isMoving = moveDirection != 0;

        _animator.SetBool(s_isMove, isMoving);

        if (isMoving)
        {
            _spriteRenderer.flipX = moveDirection < 0;
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

	private void Onlanded()
	{
        _animator.SetFloat(s_AirVelocityY, 0);
        _animator.SetBool(s_isJump, false);
    }
}
