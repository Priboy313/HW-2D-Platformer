using UnityEngine;

public class ButtonDamage : UIButtonBase
{
	[SerializeField] DevHealthHandler _healthHandler;

	protected override void Awake()
	{
		base.Awake();

		bool hasError = false;

		if (_healthHandler == null)
		{
			Debug.LogWarning("Health Handler not set!", this);
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
