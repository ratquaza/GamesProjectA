using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string itemName; // (max 25 characters)
    [SerializeField] private float cost;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description; // (max 290 characters)

    [SerializeField] private Rarity rarity;

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.gray;
            case Rarity.Uncommon:
                return Color.green;
            case Rarity.Rare:
                return Color.blue; 
            case Rarity.Epic:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }

    public Sprite Icon => icon;
    public string ItemName => itemName;
    public string Cost => cost + "g";
    public string Description => description;
    public string ItemRarity => rarity.ToString();
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}