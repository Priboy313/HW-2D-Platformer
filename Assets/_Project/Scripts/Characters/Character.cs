using System;
using System.Collections.Generic;

using UnityEngine;

using HealthUISystem;
using Abilities;

public abstract class Character : MonoBehaviour, IDamageSource, IHealthOwner, IAbilityOwner
{
	[SerializeField] private InputEventChannel _inputChannel;

	[SerializeField] private float _healthCurrent;
	[SerializeField, Min(1)] private float _healthMax = 1;
	[SerializeField] private float _damage = 0;

	private List<Damageable> _damageableParts = new();

	private List<AbilityBase> _abilities = new();

	private List<IHealthUI> _healthUI = new();

	public float Damage => _damage;

	public event Action<Vector3> Knockbacked;

	private void Awake()
	{
		_healthCurrent = _healthMax;

		InitCharacterComponents();
		InitDamageableParts();
		InitDamageDealers();
		InitAbilities();
		InitCollector();
		InitUI();
	}

	private void InitCharacterComponents()
	{
		if (TryGetComponent<CharacterMovementHandler>(out CharacterMovementHandler movementHandler))
		{
			movementHandler.Init(this, _inputChannel);
		}
	}

	private void InitDamageableParts()
	{
		Damageable[] damageables = GetComponentsInChildren<Damageable>();

		if (damageables.Length > 0)
		{
			_damageableParts.AddRange(damageables);
		}
	}

	private void InitDamageDealers()
	{
		DamageDealer[] damagings = GetComponentsInChildren<DamageDealer>();

		if (damagings.Length > 0)
		{
			foreach (DamageDealer part in damagings)
			{
				part.Init(this);
			}
		}
	}

	private void InitAbilities()
	{
		AbilityBase[] abilities = GetComponents<AbilityBase>();

		if (abilities.Length > 0)
		{
			foreach (AbilityBase ability in abilities)
			{
				ability.Init(this, _inputChannel);
			}
		}
	}

	private void InitCollector()
	{
		Collector collector = GetComponentInChildren<Collector>();

		if (collector != null)
		{
			collector.Init(this);
		}
	}

	private void InitUI()
	{
		IHealthUI[] healthUI = GetComponentsInChildren<IHealthUI>();

		if (healthUI.Length > 0)
		{
			_healthUI.AddRange(healthUI);
			UpdateUIHealth();
		}
	}

	private void OnEnable()
	{
		foreach (Damageable part in _damageableParts)
		{
			part.DamageTaken += OnDamageTaken;
		}
	}

	private void OnDisable()
	{
		foreach (Damageable part in _damageableParts)
		{
			part.DamageTaken -= OnDamageTaken;
		}
	}

	protected virtual void OnDamageTaken(float damage, Vector3 sourcePosition, bool canKnockback)
	{
		_healthCurrent -= damage;
		UpdateUIHealth();
		
		if (_healthCurrent <= 0)
		{
			Die();
		}
		else
		{
			if (canKnockback)
			{
				Knockbacked?.Invoke(sourcePosition);
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
			UpdateUIHealth();
		}
	}

	public void OnDamageTaken(float value)
	{
		OnDamageTaken(value, transform.position, false);
	}

	private void UpdateUIHealth()
	{
		if (_healthUI.Count > 0)
		{
			foreach (var ui in _healthUI)
			{
				ui.SetHealth(_healthCurrent, _healthMax);
			}
		}
	}
}
