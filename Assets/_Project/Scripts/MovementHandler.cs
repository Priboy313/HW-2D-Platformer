using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : MonoBehaviour
{
    
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _jumpForce = 9f;
    [SerializeField] private float _jumpInAirForce = 7f;
    [SerializeField] private int _jumpsCount = 0;
    [SerializeField] private int _jumpsCountMax = 1;

    [Header("Gravity Settings")]
    [SerializeField] private float _defaultGravityScale = 1.5f;
    [SerializeField] private float _fallingGravityMultiplier = 2f;

	private IInput _input;
    private OnGroundTrigger _onGroundTrigger;
    public Rigidbody2D Rigidbody { get; private set; }
    public float InputDirection { get; private set; } = 0;
    public bool IsOnGround { get; private set; } = true;

    public event Action ActionJump;
    public event Action ActionLanded;

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

        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = _defaultGravityScale;
    }

    private void OnEnable()
    {
        _input.ActionMove += OnInputMove;
        _input.ActionJump += OnInputJump;
        _onGroundTrigger.ActionOnGround += OnGroundStateChanged;
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.ActionMove -= OnInputMove;
            _input.ActionJump -= OnInputJump;
        }

        if (_onGroundTrigger != null)
        {
            _onGroundTrigger.ActionOnGround -= OnGroundStateChanged;
        }
    }

	private void FixedUpdate()
	{
        Rigidbody.gravityScale = Rigidbody.velocity.y >= 0 ? _defaultGravityScale : _defaultGravityScale * _fallingGravityMultiplier;

        Rigidbody.velocity = new Vector2(InputDirection * _moveSpeed, Rigidbody.velocity.y);
	}

    private void OnInputMove(float direction)
    {
        InputDirection = direction;
    }

    private void OnInputJump()
    {
        if (IsOnGround)
        {
            Jump(_jumpForce);
        }
        else if (_jumpsCount > 0)
        {
            Jump(_jumpInAirForce);
            _jumpsCount--;
        }
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
}
