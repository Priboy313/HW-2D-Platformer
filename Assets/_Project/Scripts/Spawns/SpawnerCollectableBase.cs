public abstract class SpawnerCollectableBase<T> : SpawnerBase<T> where T : CollectableBase
{
	protected override void InitilazeObject(T instance, SpawnPoint spawnPoint)
	{
		instance.Init(spawnPoint);
	}

	protected override void SubscribeToReleaseEvent(T instance)
	{
		instance.ActionCollected += OnCollected;
	}

	protected virtual void OnCollected(CollectableBase collectable)
	{
		collectable.ActionCollected -= OnCollected;

		FreeSpawnPoint(collectable.SpawnPoint);
	}
}
