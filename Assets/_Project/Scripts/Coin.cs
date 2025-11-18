using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : PoolableBase
{
    public SpawnPoint SpawnPoint { get; private set; }

    public event Action<Coin> ActionCollected;

    private void Awake()
    {
        if (GetComponent<Collider2D>().isTrigger == false)
        {
            Debug.Log("Trigger not set!", this);
            enabled = false;
        }
    }

    public void Init(SpawnPoint spawnPoint)
	{
        SpawnPoint = spawnPoint;

        transform.position = spawnPoint.SpawnPosition;
        transform.rotation = Quaternion.identity;
    }

    public void Collect()
    {
        ActionCollected?.Invoke(this);
        ReleaseToPool();
    }
}
