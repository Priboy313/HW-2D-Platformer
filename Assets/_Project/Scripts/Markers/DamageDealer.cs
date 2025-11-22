using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
	[SerializeField] private float _damage = 0;
	[SerializeField] private bool _canKnockback = true;

	private IDamageSource _damageSource;
	Transform _sourceTransform;

	public void Init(IDamageSource source)
	{
		_damageSource = source;
		_sourceTransform = (_damageSource as Component).transform;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		TryDealDamage(collision.collider.gameObject);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		TryDealDamage(collision.collider.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		TryDealDamage(collision.gameObject);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		TryDealDamage(collision.gameObject);
	}

	private void TryDealDamage(GameObject target)
	{

		if (_damageSource != null && target.transform.IsChildOf(_sourceTransform))
		{
			return;
		}

		if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
		{
			damageable.TryTakeDamage(_damageSource == null ? _damage : _damageSource.Damage, transform.position, _canKnockback);

		}
	}
}
