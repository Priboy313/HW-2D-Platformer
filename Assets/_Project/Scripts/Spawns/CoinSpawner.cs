using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawning")]
	[SerializeField] private GameObject _spawnPointsRoot;
    [SerializeField] private float _spawningDelay = 2f;
    [SerializeField] private float _spawnPointCooldown = 2f;
    [SerializeField] private Coin _prefab;

    private List<SpawnPoint> _spawnPoints = new();
    private List<SpawnPoint> _spawnPointsFree = new();

    private void Awake()
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

        StartCoroutine(SpawnCoinsFromPool());
    }

    private void SetFreeSpawnPoints()
    {
        _spawnPointsFree.Clear();

        foreach (var spawnPoint in _spawnPoints)
        {
            if (spawnPoint.IsFree)
            {
                _spawnPointsFree.Add(spawnPoint);
            }
        }
    }

    private void OnCoinCollected(Coin coin)
    {
        coin.ActionCollected -= OnCoinCollected;
        StartCoroutine(SetSpawnPointCooldown(coin.SpawnPoint));
    }

    private void SpawnCoinOnFreePoint()
    {
        SetFreeSpawnPoints();

        if (_spawnPointsFree.Count == 0)
        {
            return;
        }

        SpawnPoint spawnPoint = _spawnPointsFree[DevRandom.GetNumber(_spawnPointsFree.Count)];
        spawnPoint.SetFree(false);

        var instance = PoolHandler.Instance.Get(_prefab) as Coin;

        if (instance != null)
        {
            instance.Init(spawnPoint);
            instance.ActionCollected += OnCoinCollected;
        }
    }

    private IEnumerator SpawnCoinsFromPool()
    {
        var wait = new WaitForSeconds(_spawningDelay);

        while (enabled)
        {
            yield return wait;

            SpawnCoinOnFreePoint();
        }
    }

    private IEnumerator SetSpawnPointCooldown(SpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(_spawnPointCooldown);

        spawnPoint.SetFree(true);
    }
}
