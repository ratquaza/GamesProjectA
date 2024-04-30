using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(ProjectileBehaviour))]
public class ProjectileBehaviour_Editor : Editor
{
    SerializedProperty moveBehaviour;
    SerializedProperty projectileSprite;
    SerializedProperty lifetime;
    SerializedProperty speed;
    SerializedProperty size;
    SerializedProperty damage;
    SerializedProperty destroyOnWall;
    SerializedProperty ricochetOnWall;

    void OnEnable()
    {
        moveBehaviour = serializedObject.FindProperty("moveBehaviour");
        projectileSprite = serializedObject.FindProperty("projectileSprite");
        lifetime = serializedObject.FindProperty("lifetime");
        speed = serializedObject.FindProperty("speed");
        size = serializedObject.FindProperty("size");
        damage = serializedObject.FindProperty("damage");
        destroyOnWall = serializedObject.FindProperty("destroyOnWall");
        ricochetOnWall = serializedObject.FindProperty("ricochetOnWall");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (ricochetOnWall.boolValue) GUI.enabled = false;
        EditorGUILayout.PropertyField(moveBehaviour);
        if (ricochetOnWall.boolValue) GUI.enabled = true;

        EditorGUILayout.PropertyField(projectileSprite);
        EditorGUILayout.PropertyField(lifetime);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(size);
        EditorGUILayout.PropertyField(damage);

        if (ricochetOnWall.boolValue) GUI.enabled = false;
        EditorGUILayout.PropertyField(destroyOnWall);
        if (ricochetOnWall.boolValue) GUI.enabled = true;

        if ((MoveBehaviour) moveBehaviour.enumValueIndex == MoveBehaviour.Homing || destroyOnWall.boolValue) GUI.enabled = false;
        EditorGUILayout.PropertyField(ricochetOnWall);
        if ((MoveBehaviour) moveBehaviour.enumValueIndex == MoveBehaviour.Homing || destroyOnWall.boolValue) GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}