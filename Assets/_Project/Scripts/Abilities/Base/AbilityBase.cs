using System.Collections;
using UnityEngine;

namespace Abilities {

	public abstract class AbilityBase : MonoBehaviour
	{
		[SerializeField] protected KeyCode _bindKey;
		[SerializeField] protected UISliderSmooth _uiSlider;

		[Header("Ability Settings")]
		[SerializeField] protected float _activeTime;
		[SerializeField] protected float _cooldownTime;
		[SerializeField] protected float _tickInterval = 1f;

		protected IInput _input;
		protected IAbilityOwner _owner;

		private bool _isAbilityActive = false;
		private bool _isOnCooldown = false;

		public virtual void Init(IAbilityOwner owner, InputEventChannel inputChannel = null)
		{
			bool hasErrors = false;

			if (inputChannel == null)
			{
				_input = GetComponent<IInput>();

				if (_input == null)
				{
					Debug.LogError("InputChannel not set!");
					hasErrors = true;
				}
			}
			else
			{
				_input = inputChannel;
			}

			if (hasErrors)
			{
				enabled = false;
				return;
			}

			Subcribe();
			UpdateUI(1, 1);
		}

		protected virtual void Subcribe()
		{
			_input.AbilityKeyPressed += OnAbilityKeyPressed;
		}

		protected virtual void OnAbilityKeyPressed(KeyCode key)
		{
			if (key == _bindKey)
			{
				TryActivateAbility();
			}
		}

		protected virtual void TryActivateAbility()
		{
			if (_isAbilityActive || _isOnCooldown)
			{
				return;
			}

			StartCoroutine(AbilityRoutine());
		}

		protected IEnumerator AbilityRoutine()
		{
			_isAbilityActive = true;
			OnActivate();

			float timer = _activeTime;
			float timeSinceLastTick = _tickInterval;

			OnTick();
			timeSinceLastTick = 0;

			while (timer > 0)
			{
				float deltaTime = Time.deltaTime;
				timer -= deltaTime;
				timeSinceLastTick += deltaTime;

				if (timeSinceLastTick >= _tickInterval)
				{
					OnTick();
					timeSinceLastTick = 0f;
				}

				UpdateUI(timer, _activeTime);

				yield return null;
			}

			OnDeactivate();
			_isAbilityActive = false;

			if (_cooldownTime > 0)
			{
				StartCoroutine(CooldownRoutine());
			}
			else
			{
				UpdateUI(1, 1);
			}
		}

		protected IEnumerator CooldownRoutine()
		{
			_isOnCooldown = true;
			float timer = 0f;

			while (timer < _cooldownTime)
			{
				timer += Time.deltaTime;

				UpdateUI(timer, _cooldownTime);

				yield return null;
			}

			_isOnCooldown = false;
			UpdateUI(1, 1);
		}

		protected virtual void OnActivate() { }

		protected virtual void OnDeactivate() { }

		protected virtual void OnTick() { }

		private void UpdateUI(float current, float max)
		{
			if (_uiSlider != null)
			{
				_uiSlider.SetValue(current, max);
			}
		}

		private void OnDisable()
		{
			_input.AbilityKeyPressed -= OnAbilityKeyPressed;
		}
	}
}
