using UnityEngine;

public class WaypointOnGround : WaypointBase
{
    [SerializeField] private LayerMask _groundLayer;
	[SerializeField] private float _offsetFromGround = 0.1f;
    [SerializeField] private float _raycastDistance = 10f;

    protected override void CalculatePosition()
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _groundLayer);

        if (hit.collider != null)
        {
            Position = hit.point + hit.normal * _offsetFromGround;
            transform.position = Position;
        }
        else
        {
            Debug.LogWarning("WaypointOnGround не нашел землю под собой! Используется исходная позиция.", this);
            Position = transform.position;
        }
    }
}
