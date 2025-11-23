using System;
using UnityEngine;

public class AIMobPatroller : AIMobBase
{
    [SerializeField] private AIPatrolStates _patrolState;
    [SerializeField] private Path _path;

    private int _currentWaypointIndex;
    private float _reachDistance = 0.5f;
    private float _reachDistanceSqr;

    public override event Action<float> ActionMove;

    private void Awake()
    {
        bool hasErros = false;

        if (_patrolState == AIPatrolStates.OnPath)
        {
            if (_path == null)
            {
                Debug.LogError("Path to patrol not set!");
                hasErros = true;
            }

            if (_path.WaypointsCount == 0)
            {
                Debug.LogError("Waypoints on patrol path not set!", _path);
                hasErros = true;
            }
        }

        enabled = !hasErros;
        _reachDistanceSqr = _reachDistance * _reachDistance;
        enabled = false;
    }

    public override void AIUpdate()
	{
        switch (_patrolState)
        {
            case AIPatrolStates.OnPath:
                OnPathPatrolState();
                break;
            case AIPatrolStates.BetweenColliders:
            case AIPatrolStates.OnPlatform:
            case AIPatrolStates.OnEdge:
            default:
                enabled = false;
                break;
        }
    }

    private void OnPathPatrolState()
    {
        if (_path.TryGetWaypoint(_currentWaypointIndex, out WaypointBase waypoint))
        {
            Vector2 targetPosition = waypoint.Position;
            Vector2 currentPosition = transform.position;

            float sqrDistance = (targetPosition - currentPosition).sqrMagnitude;

            if (sqrDistance < _reachDistanceSqr)
            {
                ActionMove?.Invoke(0f);
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _path.WaypointsCount;
            }
            else
            {
                float moveDirection = Mathf.Sign(targetPosition.x - currentPosition.x);
                ActionMove?.Invoke(moveDirection);
            }
        }
        else
        {
            ActionMove?.Invoke(0f);
        }
    }

    private void OnBetweenCollidersState()
    {

    }
}
