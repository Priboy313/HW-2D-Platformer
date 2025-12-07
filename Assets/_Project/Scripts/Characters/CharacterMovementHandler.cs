using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Character))]
public class CharacterMovementHandler : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _moveSpeed = 3f;
	
    [Header("Jump")]
	[SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _jumpInAirHeight = 3f;
    [SerializeField] private int _jumpsCountMax = 1;

    [Header("Knockback")]
    [SerializeField] private float _knockbackForce = 9f;
    [SerializeField, Range(0.01f, 1f)] private float _knockbackJumpForce = 0.2f;
    [SerializeField] private float _knockbackDuration = .4f;

    [Header("Gravity")]
    [SerializeField] private float _defaultGravityScale = 2f;
    [SerializeField] private float _fallingGravityMultiplier = 2f;

    private int _jumpsCount = 0;
	private bool _isKnockedBack = false;

	private IInput _input;
    private Character _character;
    private OnGroundTrigger _onGroundTrigger;
    public Rigidbody2D Rigidbody { get; private set; }

    public bool IsFreeAndReady 
    { 
        get
        {
            return !_isKnockedBack;
        }
    }

	public float InputDirection { get; private set; } = 0;
    public bool IsOnGround { get; private set; } = true;

    public event Action ActionJump;
    public event Action ActionLanded;
    public event Action ActionKnockback;

    private void Awake()
    {
        bool hasErrors = false;

        _input = GetComponent<IInput>();

        if (_input == null)
        {
            _input = InputReader.Instance;

            if (_input == null)
            {
                Debug.LogError("InputReader not set!");
                hasErrors = true;
            }
        }

        _onGroundTrigger = GetComponentInChildren<OnGroundTrigger>();

        if (_onGroundTrigger == null)
        {
            Debug.LogError("OnGround Trigger not set!");
            hasErrors = true;
        }

        enabled = !hasErrors;

        _character = GetComponent<Character>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = _defaultGravityScale;
    }

    private void OnEnable()
    {
        _input.ActionMove += OnInputMove;
        _input.ActionJump += OnInputJump;
        _character.ActionKnockback += OnKnockback;
        _onGroundTrigger.ActionOnGround += OnGroundStateChanged;
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.ActionMove -= OnInputMove;
            _input.ActionJump -= OnInputJump;
        }

        if (_character != null)
        {
            _character.ActionKnockback -= OnKnockback;
        }

        if (_onGroundTrigger != null)
        {
            _onGroundTrigger.ActionOnGround -= OnGroundStateChanged;
        }
    }

	private void FixedUpdate()
	{
        Rigidbody.gravityScale = Rigidbody.velocity.y >= 0 ? _defaultGravityScale : _defaultGravityScale * _fallingGravityMultiplier;

        if (IsFreeAndReady)
        {
            Rigidbody.velocity = new Vector2(InputDirection * _moveSpeed, Rigidbody.velocity.y);
        }
	}

    private void OnInputMove(float direction)
    {
        InputDirection = direction;
    }

    private void OnInputJump()
    {
        if (IsOnGround)
        {
            Jump(GetJumpForce(_jumpHeight));
        }
        else if (_jumpsCount > 0)
        {
            Jump(GetJumpForce(_jumpInAirHeight));
            _jumpsCount--;
        }
    }

    private float GetJumpForce(float height)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y * _defaultGravityScale);
        
        // v = sqrt(2 * g * h)
        float velocity = Mathf.Sqrt(2 * gravity * height);

        return velocity * Rigidbody.mass;
    }

    private void OnKnockback(Vector3 damageSourcePosition)
    {
        Vector2 pushingDirection = new Vector2();
        pushingDirection = transform.position - damageSourcePosition;
        pushingDirection.y = 0;
        pushingDirection = (pushingDirection.normalized + Vector2.up * _knockbackJumpForce).normalized;

        Rigidbody.velocity = Vector2.zero;

        StartCoroutine(OnKnockbackRoutine());

        Rigidbody.AddForce(pushingDirection * _knockbackForce, ForceMode2D.Impulse);
    }

    private void Jump(float force)
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        Rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        ActionJump?.Invoke();
    }

    private void OnGroundStateChanged(bool isOnGround)
    {
        if (isOnGround)
        {
            _jumpsCount = _jumpsCountMax;
            ActionLanded?.Invoke();
        }

        IsOnGround = isOnGround;
    }

    private IEnumerator OnKnockbackRoutine()
    {
        _isKnockedBack = true;
        ActionKnockback?.Invoke();

        yield return new WaitForSeconds(_knockbackDuration);

        _isKnockedBack = false;
    }
}
