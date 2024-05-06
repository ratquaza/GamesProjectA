using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DungeonRoom))]
public class DungeonRoom_Editor : Editor
{
    SerializedProperty wallTilemap;
    SerializedProperty triggerCollider;
    SerializedProperty quadrants;
    SerializedProperty spawns;
    bool[] showQuadrants;

    void OnEnable()
    {
        wallTilemap = serializedObject.FindProperty("wallTilemap");
        triggerCollider = serializedObject.FindProperty("triggerCollider");
        quadrants = serializedObject.FindProperty("borders");
        spawns = serializedObject.FindProperty("spawns");
        showQuadrants = new bool[quadrants.arraySize];
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(triggerCollider);
        EditorGUILayout.PropertyField(wallTilemap);
        EditorGUILayout.PropertyField(spawns);
        if (wallTilemap.objectReferenceValue != null)
        {
            BoundsInt cellBounds = ((Tilemap) wallTilemap.objectReferenceValue).cellBounds;
            double width = Math.Ceiling((double) cellBounds.size.x/DungeonManager.ROOM_WIDTH);
            double height = Math.Ceiling((double) cellBounds.size.y/DungeonManager.ROOM_HEIGHT);

            if (GUILayout.Button("Recalculate Perimeter"))
            {
                ((Tilemap) wallTilemap.objectReferenceValue).CompressBounds();

                List<Quadrant> newArr = new List<Quadrant>();
                for (int x = 0; x < width; x++)
                { 
                    for (int y = 0; y < height; y++) {
                        if (x != 0 && x != width -1 && y != 0 && y != height -1) continue;
                        newArr.Add(new() { position = new Vector2Int(x, y) });
                    }
                };
                quadrants.SetUnderlyingValue(newArr.ToArray());
                showQuadrants = new bool[newArr.Count];
            }

            if (triggerCollider.objectReferenceValue != null && GUILayout.Button("Align Box Trigger"))
            {
                BoxCollider2D collider = triggerCollider.objectReferenceValue as BoxCollider2D;
                collider.offset = new Vector2(cellBounds.size.x/2f, cellBounds.size.y/2f);
                collider.size = new Vector2(
                    (float) Math.Ceiling((float) cellBounds.size.x/DungeonManager.ROOM_WIDTH) * DungeonManager.ROOM_WIDTH - 1,
                    (float) Math.Ceiling((float) cellBounds.size.y/DungeonManager.ROOM_HEIGHT) * DungeonManager.ROOM_HEIGHT - 1
                );
                collider.isTrigger = true;
                EditorUtility.SetDirty(serializedObject.targetObject);
            }

            for (int i = 0; i < quadrants.arraySize; i++)
            {
                SerializedProperty struc = quadrants.GetArrayElementAtIndex(i);
                SerializedProperty position = struc.FindPropertyRelative("position");
                Vector2Int quadPos = (Vector2Int) position.GetUnderlyingValue();
                showQuadrants[i] = EditorGUILayout.Foldout(showQuadrants[i], $"Quadrant {quadPos.x} {quadPos.y}");
                if (showQuadrants[i])
                {
                    if (quadPos.y == height - 1) EditorGUILayout.PropertyField(struc.FindPropertyRelative("northExit"));
                    if (quadPos.y == 0) EditorGUILayout.PropertyField(struc.FindPropertyRelative("southExit"));
                    if (quadPos.x == 0) EditorGUILayout.PropertyField(struc.FindPropertyRelative("westExit"));
                    if (quadPos.x == width - 1) EditorGUILayout.PropertyField(struc.FindPropertyRelative("eastExit"));
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}