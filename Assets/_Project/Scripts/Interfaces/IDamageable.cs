using UnityEngine;

public interface IDamageable
{
	public bool TryTakeDamage(float damage, Vector3 damageSourcePosition, bool canKnockback = false);
}
