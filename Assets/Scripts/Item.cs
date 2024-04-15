using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string itemName; // (max 25 characters)
    [SerializeField] private float cost;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description; // (max 290 characters)

    [SerializeField] private Rarity rarity;
    public enum Rarity
    {
        Common,
        Uncommon,
        Uncommoner,
        Uncommonest,
    }

    [SerializeField] private WeaponType weaponType;
    public enum WeaponType
    {
        Melee,
        Magic,
        Specialist,
        Meme,
    }

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.gray;
            case Rarity.Uncommon:
                return Color.green;
            case Rarity.Uncommoner:
                return Color.blue; 
            case Rarity.Uncommonest:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }

    public Color GetWeaponTypeColor()
    {
        switch (weaponType)
        {
            case WeaponType.Melee:
                return Color.blue;
            case WeaponType.Magic:
                return new Color(0.5f, 0f, 0.5f); // Purple
            case WeaponType.Specialist:
                return new Color(1f, 0.5f, 0.5f); // Pink
            case WeaponType.Meme:
                return new Color(1f, 0.5f, 0f); // Orange
            default:
                return Color.gray;
        }
    }



    public Sprite Icon => icon;
    public string ItemName => itemName;
    public string Cost => cost + "g";
    public string Description => description;
    public string ItemRarity => rarity.ToString();
    public string ItemType => weaponType.ToString();

}
