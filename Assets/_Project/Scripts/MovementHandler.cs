using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : MonoBehaviour
{
	[SerializeField] private InputReader _input;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _jumpForce = 5f;

    private float _inputDirection = 0;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        if (_input == null)
        {
            Debug.LogError("Input not set!");
            enabled = false;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.ActionMove += OnInputMove;
        _input.ActionJump += OnInputJump;
    }

    private void OnDisable()
    {
        _input.ActionMove -= OnInputMove;
        _input.ActionJump -= OnInputJump;
    }

	private void FixedUpdate()
	{
        _rigidbody.velocity = new Vector2(_inputDirection * _moveSpeed, _rigidbody.velocity.y);
	}

    private void OnInputMove(float direction)
    {
        _inputDirection = direction;
    }

    private void OnInputJump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
