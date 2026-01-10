using UnityEngine;

namespace HealthUISystem.Debug {
	public class DevHealthHandler : MonoBehaviour
	{
		[SerializeField] private GameObject _target;

		private IHealthOwner _healthOwner;

		private void Awake()
		{
			if (_target.TryGetComponent<IHealthOwner>(out IHealthOwner owner))
			{
				_healthOwner = owner;
			}

			if (_healthOwner == null)
			{
				UnityEngine.Debug.LogError("Character not set!", this);
				enabled = false;
				return;
			}
		}

		public void GiveHealth(float value = 1)
		{
			_healthOwner.OnHealTaken(value);
		}

		public void GiveDamage(float value = 1)
		{
			_healthOwner.OnDamageTaken(value);
		}
	}
}
