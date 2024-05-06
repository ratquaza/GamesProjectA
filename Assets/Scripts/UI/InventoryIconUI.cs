using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector2 direction;
    [SerializeField] private InventoryUI inventory;
    private PlayerLiving player { get => PlayerLiving.Instance; }
    [SerializeField] private Item item;
    [SerializeField] private Image iconImg;

    private Vector2 basePos;
    private bool isHovering = false;

    void Start()
    {
        direction = direction.normalized * 10f;
        basePos = rect.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    void Update()
    {
        Vector2 pos = isHovering ? basePos + direction : basePos;
        rect.localPosition = Vector2.Lerp(rect.localPosition, pos, Time.deltaTime * 15f);
    }

    void OnDisable()
    {
        rect.localPosition = basePos;
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        inventory.ShowUI(false);
        if (item != null && item is WeaponItem) player.EquipWeapon(item as WeaponItem);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        iconImg.sprite = item == null ? null : item.Icon;
        iconImg.color = item == null ? new Color(1,1,1,0) : new Color(1,1,1,1);
    }
}
