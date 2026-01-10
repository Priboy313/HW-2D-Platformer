
namespace HealthUISystem {
	public interface IHealthOwner
	{
		public void OnHealTaken(float value);

		public void OnDamageTaken(float value);
	}
}
