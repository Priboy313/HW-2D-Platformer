using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour, IDamageable
{
	[SerializeField] private float _takingDelay = 1f;

	private bool _canTakeDamage = true;

	public event Action<float, Vector3, bool> ActionDamageTaken;

	private void OnEnable()
	{
		_canTakeDamage = true;
	}

	public void TryTakeDamage(float damage, Vector3 damageSourcePosition, bool canKnockback)
	{
		StartCoroutine(TakeDamageRoutine(damage, damageSourcePosition, canKnockback));
	}

	private IEnumerator TakeDamageRoutine(float damage, Vector3 damageSourcePosition, bool canKnockback)
	{
		if (_canTakeDamage)
		{
			_canTakeDamage = false;

			ActionDamageTaken?.Invoke(damage, damageSourcePosition, canKnockback);

			yield return new WaitForSeconds(_takingDelay);

			_canTakeDamage = true;
		}
	}
}
