using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{
    private Character _owner;

    public void Init(Character owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_owner != null)
        {
            if (collision.TryGetComponent<ICollectable>(out ICollectable collectable))
            {
                collectable.Collect(_owner.gameObject);
            }
        }
    }
}
