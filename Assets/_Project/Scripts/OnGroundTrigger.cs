using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnGroundTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    private int _groundContacts = 0;

    private Rigidbody2D _rigidbody;
    private Collider2D _trigger;

	public event Action<bool> ActionOnGround;

	private void Awake()
	{
        bool hasErrors = false;

		if (GetComponent<Collider2D>().isTrigger == false)
		{
			Debug.LogError("Trigger not set!");
            hasErrors = true; ;
		}

        _rigidbody = GetComponentInParent<Rigidbody2D>();

        if (_rigidbody == null)
        {
            Debug.LogError("Parent Rigidbody2D not found!");
            hasErrors = true;
        }

        enabled = !hasErrors;
        _trigger = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rigidbody.velocity.y <= 0 && (_groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (_groundContacts == 0)
            {
                ActionOnGround?.Invoke(true);
            }

            _groundContacts++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            _groundContacts--;

            _groundContacts = Mathf.Max(0, _groundContacts);

            if (_groundContacts == 0)
            {
                ActionOnGround?.Invoke(false);
            }
        }
    }
}
