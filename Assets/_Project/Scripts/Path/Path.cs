using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public int WaypointsCount => Waypoints.Count;

    private List<WaypointBase> _waypoints;

    public List<WaypointBase> Waypoints 
    {
        get
        {
            if (_waypoints == null)
            {
                InitWaypoints();
            }

            return _waypoints;
        }
    }

    private void InitWaypoints()
    {
        _waypoints = new List<WaypointBase>();
        WaypointBase[] childrensWaypoints = GetComponentsInChildren<WaypointBase>();

        if (childrensWaypoints.Length > 0)
        {
            Waypoints.AddRange(childrensWaypoints);
        }
        else
        {
            Debug.LogWarning("Waypoints not set", this);
            enabled = false;
        }
    }

    public bool TryGetWaypoint(int index, out WaypointBase waypoint)
    {
        if (WaypointsCount == 0)
        {
            waypoint = null;
            return false;
        }

        waypoint = Waypoints[index % WaypointsCount];
        return true;
    }
}
