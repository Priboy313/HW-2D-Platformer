using UnityEngine;

public abstract class WaypointBase : MonoBehaviour
{
    public Vector2 Position { get; protected set; }

    protected virtual void Awake()
    {
        CalculatePosition();
    }

    protected abstract void CalculatePosition();
}
