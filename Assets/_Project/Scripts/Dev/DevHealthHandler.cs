using UnityEngine;

public class DevHealthHandler : MonoBehaviour
{
	[SerializeField] private Character _character;

	private void Awake()
	{
		if (_character == null)
		{
			Debug.LogError("Character not set!", this);
			enabled = false;
			return;
		}
	}

	public void GiveHealth(float value = 1)
	{
		_character.OnHealTaken(value);
	}

	public void GiveDamage(float value = 1)
	{
		_character.OnDamageTaken(value);
	}
}
