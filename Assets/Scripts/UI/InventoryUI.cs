using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private PlayerLiving player { get => PlayerLiving.Instance; }

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject radialRoot;

    [SerializeField] private InventoryIconUI topLeftIcon;
    [SerializeField] private InventoryIconUI topRightIcon;
    [SerializeField] private InventoryIconUI botLeftIcon;
    [SerializeField] private InventoryIconUI botRightIcon;
    [SerializeField] private InventoryIconUI botIcon;

    private RectTransform rect;
    private PlayerActions actions;

    void Start()
    {
        actions = PlayerLiving.Instance.actions;
        actions.Inventory.Enable();
        actions.Inventory.ToggleInventory.performed += (ctx) => {
            ShowUI(!radialRoot.activeInHierarchy);
        };

        rect = GetComponent<RectTransform>();

        WeaponItem[] weapons = player.GetWeapons();
        topLeftIcon.SetItem(weapons[0]);
        topRightIcon.SetItem(weapons[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerLiving.Instance != null){
            rect.position = Camera.main.WorldToScreenPoint(player.transform.position);
        }
        
    }

    public void ShowUI(bool show)
    {
        WeaponItem[] weapons = player.GetWeapons();
        topLeftIcon.SetItem(weapons[0]);
        topRightIcon.SetItem(weapons[1]);

        cameraFollow.SetFollowMouse(!show);
        radialRoot.SetActive(show);
    }
}
