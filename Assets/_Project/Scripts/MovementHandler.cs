using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : MonoBehaviour
{
    [Header("Components")]
	[SerializeField] private InputReader _input;
    [SerializeField] private OnGroundTrigger _onGroundTrigger;
    
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private int _jumpsCount = 0;
    [SerializeField] private int _jumpsCountMax = 1;

    public Rigidbody2D Rigidbody { get; private set; }
    public float InputDirection { get; private set; } = 0;
    public bool IsOnGround { get; private set; } = true;

    public event Action ActionJump;
    public event Action ActionLanded;

    private void Awake()
    {
        bool hasErrors = false;

        if (_input == null)
        {
            Debug.LogError("Input not set!");
            hasErrors = true;
        }

        if (_onGroundTrigger == null)
        {
            Debug.LogError("OnGround Trigger not set!");
            hasErrors = true;
        }

        enabled = !hasErrors;

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.ActionMove += OnInputMove;
        _input.ActionJump += OnInputJump;
        _onGroundTrigger.ActionOnGround += OnGroundStateChanged;
    }

    private void OnDisable()
    {
        _input.ActionMove -= OnInputMove;
        _input.ActionJump -= OnInputJump;
        _onGroundTrigger.ActionOnGround -= OnGroundStateChanged;
    }

	private void FixedUpdate()
	{
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
            Jump();
        }
        else if (_jumpsCount > 0)
        {
            Jump();
            _jumpsCount--;
        }
    }

    private void Jump()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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
