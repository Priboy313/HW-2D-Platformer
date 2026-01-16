using HealthUISystem;
using UnityEngine;

namespace Abilities {

	public class VampiricAuraAbility : AuraAbilityBase
	{
		[Header("Vampiric Stats")]
		[SerializeField] private float _damagePerTick = 1f;

		private IHealthOwner _healthOwner;

		public override void Init(IAbilityOwner owner, InputEventChannel inputChannel = null)
		{
			base.Init(owner, inputChannel);

			if (owner is IHealthOwner healthOwner)
			{
				_healthOwner = healthOwner;
			}
			else if (owner is Component componentOwner)
			{
				_healthOwner = componentOwner.GetComponent<IHealthOwner>();
			}

			if (_healthOwner == null)
			{
				Debug.LogWarning("[Owner does not implement IHealthOwner! Vampirism won't work.", this);
			}
		}

		protected override void OnTick()
		{
			if (_targetsInRange.Count == 0)
			{
				return;
			}

			if (_healthOwner == null)
			{
				return;
			}

			IDamageable nearestTarget = GetNearestTarget();

			if (nearestTarget != null)
			{
				if (nearestTarget.TryTakeDamage(_damagePerTick, transform.position))
				{
					_healthOwner.OnHealTaken(_damagePerTick);
				}
			}
		}

		private IDamageable GetNearestTarget()
		{
			IDamageable nearestTarget = null;
			float closesDistanceSqr = Mathf.Infinity;
			Vector3 currentPosition = transform.position;

			for (int i = _targetsInRange.Count - 1; i >= 0; i--)
			{
				var target = _targetsInRange[i];

				if (target == null || (target as Component) == null)
				{
					_targetsInRange.RemoveAt(i);
					continue;
				}

				Transform targetTransform = (target as Component).transform;
				Vector3 directionToTarget = targetTransform.position - currentPosition;
				float distSqrToTarget = directionToTarget.sqrMagnitude;

				if (distSqrToTarget < closesDistanceSqr)
				{
					closesDistanceSqr = distSqrToTarget;
					nearestTarget = target;
				}
			}


			return nearestTarget;
		}
	}
}
