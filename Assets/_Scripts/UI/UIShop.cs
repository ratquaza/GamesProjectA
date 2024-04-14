using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    private Transform container;
    private Transform itemTemplate;
    Dictionary<string, Item> items;
    private string priceText = "PriceText";
    private string nameText = "NameText";

    

    private void Awake()
    {
        container = transform.Find("ItemsContainer");
        itemTemplate = container.Find("ItemTemplate");
        itemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        items = ItemDatabase.Instance.GetItems();

        if (items != null)
        {
            InstantiateItemButton(items.ElementAt(Random.Range(0, items.Count)).Value.name, 50, 1); 
            InstantiateItemButton(items.ElementAt(Random.Range(0, items.Count)).Value.name, 60, 2); 
            InstantiateItemButton(items.ElementAt(Random.Range(0, items.Count)).Value.name, 80, 3); 
        }

    }



    private void InstantiateItemButton(string itemName, float itemCost, int positionIndex)
    {
        Transform itemTemplateTransform = Instantiate(itemTemplate, container);
        RectTransform itemTemplateRectTransform = itemTemplate.GetComponent<RectTransform>();

        float itemUIHeight = 90f;
        itemTemplateRectTransform.anchoredPosition = new Vector2(0, -itemUIHeight * positionIndex);

        itemTemplateTransform.Find(nameText).GetComponent<TextMeshProUGUI>().SetText(itemName);
        itemTemplateTransform.Find(priceText).GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        itemTemplateTransform.gameObject.SetActive(true);

        itemTemplateTransform.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(itemName, itemCost));
    }

    private void OnButtonClick(string itemName, float itemCost)
    {
        Debug.Log("Button clicked! Item Name: " + itemName + ", Item Cost: " + itemCost);   
    }

}
