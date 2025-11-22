using UnityEngine;

public interface IDamageable
{
	public void TryTakeDamage(float damage, Vector3 damageSourcePosition, bool canKnockback);
}
