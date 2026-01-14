using UnityEngine;

namespace Abilities {

	public class AuraAbilityBase : AbilityBase
	{
		[SerializeField] protected CircleCollider2D _collider;

		[Header("Collider Settings")]
		[SerializeField] protected bool _isTrigger = true;
		[SerializeField] protected float _colliderWidth = 0;
		[SerializeField] protected Vector2 _colliderOffset = new Vector2(0, 0);

	}
}
