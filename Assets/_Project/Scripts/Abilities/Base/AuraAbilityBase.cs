using System.Collections.Generic;
using UnityEngine;

namespace Abilities {

	public class AuraAbilityBase : AbilityBase
	{
		[SerializeField] protected AbilityCircleCollider _auraPrefab;

		[Header("Aura Config")]
		[SerializeField] protected LayerMask _targetLayer;
		[SerializeField] protected bool _isTrigger = true;
		[SerializeField] protected float _radius = 3f;
		[SerializeField] protected bool _hasVisual = true;
		[SerializeField] protected Color _visualColor;

		protected AbilityCircleCollider _currentAuraInstance;
		protected List<IDamageable> _targetsInRange = new();

		protected void OnValidate()
		{
			if (_auraPrefab != null)
			{
				CircleCollider2D collider = _auraPrefab.GetComponent<CircleCollider2D>();
				SpriteRenderer sprite = _auraPrefab.GetComponent<SpriteRenderer>();

				collider.isTrigger = _isTrigger;
				_auraPrefab.transform.localScale = Vector3.one * _radius;

				sprite.enabled = _hasVisual;
				sprite.color = _visualColor;
			}
		}

		protected override void OnActivate()
		{
			_targetsInRange.Clear();

			if (_auraPrefab != null)
			{
				_currentAuraInstance = Instantiate(_auraPrefab, transform);
				ConfigureAura(_currentAuraInstance);

				_currentAuraInstance.TriggerEntered += OnTargetEnter;
				_currentAuraInstance.TriggerExited += OnTargetExited;
			}
			else
			{
				Debug.LogError("Aura Prefab not set!", this);
			}
		}

		protected override void OnDeactivate()
		{
			if (_currentAuraInstance != null)
			{
				_currentAuraInstance.TriggerEntered -= OnTargetEnter;
				_currentAuraInstance.TriggerExited -= OnTargetExited;

				Destroy(_currentAuraInstance.gameObject);
				_currentAuraInstance = null;
			}

			_targetsInRange.Clear();
		}

		protected void ConfigureAura(AbilityCircleCollider aura)
		{
			var collider = aura.GetComponent<CircleCollider2D>();

			if (collider != null)
			{
				collider.isTrigger = _isTrigger;
			}

			aura.transform.localScale = Vector3.one * _radius;
			aura.transform.localPosition = Vector3.zero;

			if (_hasVisual)
			{
				var sprite = aura.GetComponent<SpriteRenderer>();
				if (sprite != null)
				{
					sprite.color = _visualColor;
					sprite.enabled = true;
				}
			}
		}

		protected void OnTargetEnter(Collider2D collision)
		{
			if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
			{
				if (collision.TryGetComponent<IDamageable>(out var target))
				{
					_targetsInRange.Add(target);
				}
			}
		}

		protected void OnTargetExited(Collider2D collision)
		{
			if (collision.TryGetComponent<IDamageable>(out var target))
			{
				if (_targetsInRange.Contains(target))
				{
					_targetsInRange.Remove(target);
				}
			}
		}
	}
}
