using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageSource
{
	[SerializeField] private float _healthCurrent;
	[SerializeField, Min(1)] private float _healthMax = 1;
	[SerializeField] private float _damage = 0;

	private List<Damageable> _damageableParts;

	public float Damage => _damage;

	public event Action<Vector3> ActionKnockback;

	private void Awake()
	{
		_healthCurrent = _healthMax;

		Damageable[] damageables = GetComponentsInChildren<Damageable>();

		if (damageables.Length > 0)
		{
			_damageableParts = new();
			_damageableParts.AddRange(damageables);
		}
		else
		{
			Debug.LogError("Damageable parts not set!", this);
			enabled = false;
		}

		DamageDealer[] damagings = GetComponentsInChildren<DamageDealer>();

		if (damagings.Length > 0)
		{
			foreach (DamageDealer part in damagings)
			{
				part.Init(this);
			}
		}

		Collector collector = GetComponentInChildren<Collector>();

		if (collector != null)
		{
			collector.Init(this);
		}
	}

	private void OnEnable()
	{
		foreach (Damageable part in _damageableParts)
		{
			part.ActionDamageTaken += OnDamageTaken;
		}
	}

	private void OnDisable()
	{
		foreach (Damageable part in _damageableParts)
		{
			part.ActionDamageTaken -= OnDamageTaken;
		}
	}

	protected virtual void OnDamageTaken(float damage, Vector3 sourcePosition, bool canKnockback)
	{
		_healthCurrent -= damage;
		
		if (_healthCurrent <= 0)
		{
			Die();
		}
		else
		{
			if (canKnockback)
			{
				ActionKnockback?.Invoke(sourcePosition);
			}
		}
	}

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

	public virtual void OnHealTaken(float heal)
	{
		if (heal > 0)
		{
			float newHealth = _healthCurrent + heal;
			_healthCurrent = newHealth > _healthMax ? _healthMax : newHealth;
		}
	}
}
