using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnGroundTrigger : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rigidbody.velocity.y <= 0 && collision.TryGetComponent<SolidSurface>(out _))
        {
            ActionOnGround?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SolidSurface>(out _))
        {
            ActionOnGround?.Invoke(false);
        }
    }
}
