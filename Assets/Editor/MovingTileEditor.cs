using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingTile))]
public class MovingTileEditor : Editor
{
    private void OnSceneGUI()
    {
        MovingTile movingTile = target as MovingTile;

        // Draw coordinate axes for each waypoint
        for (int i = 0; i < movingTile.waypoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newWaypointPosition = Handles.PositionHandle(movingTile.waypoints[i].position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(movingTile, "Move Waypoint");
                movingTile.waypoints[i].position = newWaypointPosition;
            }
        }
    }
}
