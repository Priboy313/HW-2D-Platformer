using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class CollectableBase : PoolableBase, ICollectable
{
	public SpawnPoint SpawnPoint { get; protected set; }

	public virtual event Action<CollectableBase> ActionCollected;

	protected void Awake()
	{
		if (GetComponent<Collider2D>().isTrigger == false)
		{
			Debug.Log("Trigger not set!", this);
			enabled = false;
		}
	}

	public virtual void Init(SpawnPoint spawnPoint)
	{
		SpawnPoint = spawnPoint;

		transform.position = spawnPoint.SpawnPosition;
		transform.rotation = Quaternion.identity;
	}

	public virtual void Collect(GameObject collector)
	{
		ActionCollected?.Invoke(this);
		ReleaseToPool();
	}
}
