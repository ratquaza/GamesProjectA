using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(ShootBehaviour))]
public class ShootBehaviour_Editor : Editor
{
    SerializedProperty spawnBehaviour;
    SerializedProperty delay;
    SerializedProperty shootCount;
    SerializedProperty delayBetweenShoots;
    SerializedProperty spinSpeed;
    SerializedProperty gap;

    void OnEnable()
    {
        spawnBehaviour = serializedObject.FindProperty("spawnBehaviour");
        delay = serializedObject.FindProperty("delay");
        shootCount = serializedObject.FindProperty("shootCount");
        delayBetweenShoots = serializedObject.FindProperty("delayBetweenShoots");
        spinSpeed = serializedObject.FindProperty("spinSpeed");
        gap = serializedObject.FindProperty("gap");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnBehaviour);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(shootCount);

        switch ((SpawnerType) spawnBehaviour.enumValueIndex)
        {
            case SpawnerType.Spinning:
                EditorGUILayout.PropertyField(spinSpeed);
                break;
            case SpawnerType.Arc:
                EditorGUILayout.PropertyField(gap);
                break;
            default:
                EditorGUILayout.PropertyField(delayBetweenShoots);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}