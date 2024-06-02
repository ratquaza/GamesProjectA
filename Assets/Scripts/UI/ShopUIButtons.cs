using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject itemContainers;

    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private GameObject welcomePanel;

    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI rarityTxt;
    [SerializeField] private TextMeshProUGUI itemTypeTxt;
    [SerializeField] private TextMeshProUGUI itemDescTxt;
    [SerializeField] private TextMeshProUGUI goldTxt;

    [SerializeField] private Image rarityImage;
    [SerializeField] private Image itemTypeImage;

    private Item selectedItem;
    private GameObject selectedCell;

    private PlayerLiving player { get => PlayerLiving.Instance; }

    Dictionary<string, Item> items;

    private void Start()
    {
        items = ItemDatabase.Instance.GetItems();

        DeactivateAllSprites();
        PopulateSprites();

        welcomePanel.SetActive(true);
        itemInfoPanel.SetActive(false);
    }

    private void PopulateSprites()
    {
        foreach (Transform cell in itemContainers.transform)
        {
            int randomNum = Random.Range(0, items.Count);
            Item randomItem = items.ElementAt(randomNum).Value;

            ItemCell itemCell = cell.GetComponent<ItemCell>();

            itemCell?.SetItemIndex(randomNum);

            foreach (Transform child in cell)
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                {
                    
                    image.sprite = randomItem.Icon;
                    image.gameObject.SetActive(true);
                }
            }
        }
    }

    void DeactivateAllSprites()
    {
        foreach (Transform cell in itemContainers.transform)
        {
            foreach (Transform child in cell)
            {
                Image image = child.GetComponent<Image>();

                image?.gameObject.SetActive(false);
            }
        }
    }

    private bool isFirstSelection = true;

    public void SelectItem(GameObject clickedCell)
    {
        if (isFirstSelection) // Only called Once
        {
            welcomePanel.SetActive(false);
            itemInfoPanel.SetActive(true);
            isFirstSelection = false;
        }

        // Save selected cell GameObject
        // Save selected Item object
        selectedCell = clickedCell;
        ItemCell itemCell = clickedCell.GetComponent<ItemCell>();
        selectedItem = items.ElementAt(itemCell.GetItemIndex()).Value;

        // Modify text elements
        itemNameTxt.text = selectedItem.ItemName;
        itemDescTxt.text = selectedItem.Description;
        goldTxt.text = selectedItem.Cost;
        rarityTxt.text = selectedItem.ItemRarity;
        rarityImage.color = selectedItem.GetRarityColor();
        itemTypeTxt.text = selectedItem is WeaponItem ? ((WeaponItem) selectedItem).GetWeaponType() : "";
        itemTypeImage.color = selectedItem is WeaponItem ? ((WeaponItem) selectedItem).GetWeaponTypeColor() : new Color(1,1,1,0);
    }

    public void PurchaseItem()
    {
        if (player == null || selectedItem == null)
        {
            Debug.LogError("Player or selected item is null.");
            return;
        }

        if (selectedItem is WeaponItem)
        {
            WeaponItem selectedWeaponItem = (WeaponItem)selectedItem;

            int itemCost = selectedWeaponItem.CostFloat;
            if (player.goldCount < itemCost)
            {
                Debug.LogWarning("Not enough gold to purchase the item.");
                return;
            }

            player.SubtractGold(itemCost);

            if (player.GiveWeapon(selectedWeaponItem))
            {
                player.EquipWeapon(selectedWeaponItem);
            }
            else
            {
                // If the player already has a weapon equipped, swap it with the purchased one
                WeaponItem oldWeapon = player.GetWeaponAt(player.GetEquippedWeaponIndex());
                player.EquipWeapon(selectedWeaponItem, true);
                selectedWeaponItem = oldWeapon;
            }
        }
    }

}
