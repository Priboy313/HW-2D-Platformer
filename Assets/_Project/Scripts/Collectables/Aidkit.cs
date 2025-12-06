using UnityEngine;

public class Aidkit : CollectableBase
{
	[SerializeField] private float _healPower = 1f;

	public override void Collect(GameObject collector)
	{
		if (collector.TryGetComponent<Character>(out Character character))
		{
			character.OnHealTaken(_healPower);
		}

		base.Collect(collector);
	}
}
