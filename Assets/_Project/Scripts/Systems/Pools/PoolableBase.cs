using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolableBase : MonoBehaviour
{
	private IObjectPool<PoolableBase> _pool;

	public void SetPool(IObjectPool<PoolableBase> pool)
	{
		_pool = pool;
	}

	protected void ReleaseToPool()
	{
		_pool.Release(this);
	}
}
