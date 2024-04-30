using System;
using UnityEditor;

[CustomEditor(typeof(ShootBehaviour))]
public class ShootBehaviour_Editor : Editor
{
    SerializedProperty spawnBehaviour;
    SerializedProperty delay;
    SerializedProperty shootCount;
    SerializedProperty delayBetweenShoots;
    SerializedProperty spinSpeed;
    SerializedProperty arcDegrees;

    void OnEnable()
    {
        spawnBehaviour = serializedObject.FindProperty("spawnBehaviour");
        delay = serializedObject.FindProperty("delay");
        shootCount = serializedObject.FindProperty("shootCount");
        delayBetweenShoots = serializedObject.FindProperty("delayBetweenShoots");
        spinSpeed = serializedObject.FindProperty("spinSpeed");
        arcDegrees = serializedObject.FindProperty("arcDegrees");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnBehaviour);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(shootCount);

        switch ((SpawnerType) spawnBehaviour.enumValueIndex)
        {
            case SpawnerType.Arc:
                EditorGUILayout.PropertyField(arcDegrees);
                if (shootCount.intValue < 2) shootCount.intValue = 2;
                break;
            case SpawnerType.Spinning:
            case SpawnerType.Spray:
                EditorGUILayout.PropertyField(spinSpeed);
                if (shootCount.intValue > 1) EditorGUILayout.PropertyField(delayBetweenShoots);
                break;
            default:
                if (shootCount.intValue > 1) EditorGUILayout.PropertyField(delayBetweenShoots);
                break;
        }

        shootCount.intValue = Math.Max(1, shootCount.intValue);

        serializedObject.ApplyModifiedProperties();
    }
}