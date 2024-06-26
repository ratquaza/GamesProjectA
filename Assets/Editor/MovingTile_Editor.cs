using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingTile))]
public class MovingTile_Editor : Editor
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

        // Draw rotation pivot handle
        EditorGUI.BeginChangeCheck();
        Vector3 newRotationPivotPosition = Handles.PositionHandle(movingTile.rotationPivot.position, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(movingTile, "Move Rotation Pivot");
            movingTile.rotationPivot.position = newRotationPivotPosition;
        }
    }
}
