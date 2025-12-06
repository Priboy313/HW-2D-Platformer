using UnityEngine;

public interface ICollectable
{
	public void Init(SpawnPoint spawnPoint);

	public void Collect(GameObject targetcollector);
}
