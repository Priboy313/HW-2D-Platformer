using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerBase<T> : MonoBehaviour where T : PoolableBase
{
	[Header("Spawning")]
	[SerializeField] private GameObject _spawnPointsRoot;
	[SerializeField] private T _prefab;
	[SerializeField] private float _spawningDelay = 2f;
	[SerializeField] private float _spawnPointCooldown = 2f;

	private List<SpawnPoint> _spawnPoints = new();
	private List<SpawnPoint> _spawnPointsFree = new();

	protected virtual void Awake()
	{
		bool hasErrors = false;

		if (_spawnPointsRoot == null)
		{
			Debug.LogError("Spawn Points Root not set!", this);
			hasErrors = true;
		}

		SpawnPoint[] spawnPoints = _spawnPointsRoot.GetComponentsInChildren<SpawnPoint>();

		if (spawnPoints.Length == 0)
		{
			Debug.LogError("Spawn Points not set!");
			hasErrors = true;
		}

		if (_prefab == null)
		{
			Debug.LogError("Prefab is not set!", this);
			hasErrors = true;
		}

		enabled = !hasErrors;

		_spawnPoints.AddRange(spawnPoints);

		StartCoroutine(SpawnRoutine());
	}

	protected abstract void InitilazeObject(T instance, SpawnPoint spawnPoint);

	protected abstract void SubscribeToReleaseEvent(T instance);

	protected void FreeSpawnPoint(SpawnPoint spawnPoint)
	{
		StartCoroutine(SetSpawnPointCooldown(spawnPoint));
	}

	private void SetFreeSpawnPoints()
	{
		_spawnPointsFree.Clear();

		foreach (var point in _spawnPoints)
		{
			if (point.IsFree)
			{
				_spawnPointsFree.Add(point);
			}
		}
	}

	private void SpawnOnFreePoint()
	{
		SetFreeSpawnPoints();

		if (_spawnPointsFree.Count == 0)
		{
			return;
		}

		SpawnPoint spawnPoint = _spawnPointsFree[DevRandom.GetNumber(_spawnPointsFree.Count)];

		var instance = PoolHandler.Instance.Get(_prefab) as T;

		if (instance != null)
		{
			spawnPoint.SetFree(false);

			InitilazeObject(instance, spawnPoint);
			SubscribeToReleaseEvent(instance);
		}
	}

	private IEnumerator SpawnRoutine()
	{
		var wait = new WaitForSeconds(_spawningDelay);

		while (enabled)
		{
			yield return wait;

			SpawnOnFreePoint();
		}
	}

	private IEnumerator SetSpawnPointCooldown(SpawnPoint spawnPoint)
	{
		yield return new WaitForSeconds(_spawnPointCooldown);

		spawnPoint.SetFree(true);
	}
}
