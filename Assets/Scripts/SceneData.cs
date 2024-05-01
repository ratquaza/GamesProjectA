using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [SerializeField] 
    public GameState sceneState;

    public enum GameState
    {
        InOverworld,
        InDungeon
    }
}