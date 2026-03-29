using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class inventory_ui : MonoBehaviour
{
    public static inventory_ui instance;
    public GameObject inventoryPanel;

    [Header("sections")]
    public Transform invContainer;
    public GameObject slotPrefab;

    [Header("equipped slots")]
    public Image weaponIcon;
    public Image armorIcon;

    [Header("details")]
    public TMP_Text detailName;
    public TMP_Text detailStats;
    public Button actionButton;
    public Button upgradeButton;

    private equipable_item selectedItem;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (pause_menu.instance != null && pause_menu.instance.pauseMenu.activeSelf) return;
            if (shop_ui.instance != null && shop_ui.instance.IsOpen()) return;
            if (role_menu.instance != null && role_menu.instance.IsOpen()) return;
            if (lvl_manager.instance != null && lvl_manager.instance.IsOpen()) return;

            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool state = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(state);

        if (manager.instance != null)
        {
            manager.instance.HUDVisible(!state);
        }

        Time.timeScale = state ? 0f : 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (state) RefreshInventory();
    }

    public void RefreshInventory()
    {
        if (invContainer == null) return;
        if (player_equipment.instance == null) return;

        //vymaze stare sloty
        foreach (Transform child in invContainer) Destroy(child.gameObject);

        //vytvori nove sloty pro kazdy item v inventari
        foreach (equipable_item item in player_equipment.instance.inventory)
        {
            if (item?.data == null) continue;

            GameObject go = Instantiate(slotPrefab, invContainer);

            //nastaveni ikony a textu levelu
            Image btnImage = go.GetComponent<Image>();
            if (btnImage != null) btnImage.sprite = item.data.itemIcon;

            TMP_Text txt = go.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = "lvl " + item.currentLevel;

            //prirazeni kliknuti na slot
            Button btn = go.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SelectItem(item));
            }
        }
        UpdateEquippedIcons();
    }

    void UpdateEquippedIcons()
    {
        if (player_equipment.instance == null) return;
        SetIcon(weaponIcon, player_equipment.instance.equippedWeapon);
        SetIcon(armorIcon, player_equipment.instance.equippedArmor);
    }

    void SetIcon(Image img, equipable_item item)
    {
        if (img == null) return;
        bool hasItem = item?.data != null;
        img.sprite = hasItem ? item.data.itemIcon : null;
        img.color = hasItem ? Color.white : new Color(1, 1, 1, 0.2f);
    }

    public void SelectItem(equipable_item item)
    {
        selectedItem = item;
        detailName.text = $"{item.data.itemName}\n(lvl {item.currentLevel})";

        string typeOfBoost = item.data.type == item_data.ItemType.Weapon ? "damage" : "hp";
        detailStats.text = $"{typeOfBoost}: +{item.GetCurrentBoost()}\nrole: {item.data.forRole}";

        //tlacitko equip/unequip
        bool isEquipped = (player_equipment.instance.equippedWeapon == item || player_equipment.instance.equippedArmor == item);
        actionButton.GetComponentInChildren<TMP_Text>().text = isEquipped ? "Unequip" : "Equip";

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => {
            if (isEquipped) player_equipment.instance.UnequipItem(item);
            else player_equipment.instance.EquipItem(item);
            RefreshInventory();
            SelectItem(item);
        });

        if (upgradeButton != null) SetupUpgradeButton(item);
    }

    void SetupUpgradeButton(equipable_item item)
    {
        TMP_Text upTxt = upgradeButton.GetComponentInChildren<TMP_Text>();
        int cost = item.GetUpgradeCost();

        bool canUpgrade = item.CanUpgrade();
        bool hasMoney = coin_manager.instance != null && coin_manager.instance.coins >= cost;

        upgradeButton.interactable = canUpgrade && hasMoney;

        if (!canUpgrade)
        {
            upTxt.text = $"Locked";
        }
        else
        {
            upTxt.text = $"Upgrade\n({cost} g)";
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => {
            item.Upgrade();
            RefreshInventory();
            SelectItem(item);
        });
    }

    public void ForceClose()
    {
        if (inventoryPanel.activeSelf) ToggleInventory();
    }

    public bool IsOpen()
    {
        return inventoryPanel != null && inventoryPanel.activeSelf;
    }
}