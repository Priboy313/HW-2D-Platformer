using UnityEngine;
using UnityEngine.UI;
using HealthUISystem.Internal;

namespace HealthUISystem.Debug {

	[RequireComponent(typeof(Button))]
	public class ButtonDamage : HealthUIButtonBase 
	{
		[SerializeField] DevHealthHandler _healthHandler;
		protected override void Awake()
		{
			base.Awake();

			bool hasError = false;

			if (_healthHandler == null)
			{
				UnityEngine.Debug.LogWarning("Health Handler not set!", this);
				hasError = true;
			}

			if (hasError)
			{
				enabled = false;
				return;
			}
		}

		protected override void OnClick()
		{
			_healthHandler.GiveDamage();
		}
	}
}
