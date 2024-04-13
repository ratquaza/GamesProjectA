using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private float cost;
}
