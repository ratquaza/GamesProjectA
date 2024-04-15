using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
}
