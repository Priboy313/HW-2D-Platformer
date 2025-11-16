using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnGroundTrigger : MonoBehaviour
{
	public event Action<bool> ActionOnGround;

	private void Awake()
	{
		if (GetComponent<Collider2D>().isTrigger == false)
		{
			Debug.LogError("Trigger not set!");
			enabled = false;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SolidSurface>(out _))
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
