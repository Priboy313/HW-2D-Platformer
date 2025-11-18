using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler : MonoBehaviour
{
	private static PoolHandler _instance;
    private readonly Dictionary<GameObject, IObjectPool<PoolableBase>> _pools = new();

    public static PoolHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolHandler>();

                if (_instance == null)
                {
                    GameObject newInputReader = new GameObject(nameof(PoolHandler));
                    _instance = newInputReader.AddComponent<PoolHandler>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public PoolableBase Get(PoolableBase prefab)
    {
        IObjectPool<PoolableBase> pool = GetPool(prefab);
        return pool.Get();
    }

    private IObjectPool<PoolableBase> GetPool(PoolableBase prefab)
    {
        if (_pools.TryGetValue(prefab.gameObject, out IObjectPool<PoolableBase> pool))
        {
            return pool;
        }

        pool = new ObjectPool<PoolableBase>(
            createFunc: () => CreatePooledObject(prefab),
            actionOnGet: (instance) => instance.gameObject.SetActive(true),
            actionOnRelease: (instance) => instance.gameObject.SetActive(false),
            actionOnDestroy: (instance) => Destroy(instance.gameObject),
            collectionCheck: true
        );

        _pools.Add(prefab.gameObject, pool);
        return pool;
    }

    private PoolableBase CreatePooledObject(PoolableBase prefab)
    {
        PoolableBase instance = Instantiate(prefab);
        instance.SetPool(GetPool(prefab));

        return instance;
    }
}
