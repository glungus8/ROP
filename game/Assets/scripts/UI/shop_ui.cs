using UnityEngine;
using System.Collections.Generic;

public class shop_ui : MonoBehaviour
{
    public static shop_ui instance;

    public GameObject shopPanel;
    public Transform container;
    public GameObject slotPrefab;
    public List<item_data> allItemsForSale;

    void Awake()
    {
        instance = this;
    }

    public void OpenShop()
    {
        if (inventory_ui.instance != null && inventory_ui.instance.IsOpen()) return;
        if (pause_menu.instance != null && pause_menu.instance.pauseMenu.activeSelf) return;

        shopPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (manager.instance != null) manager.instance.HUDVisible(false);

        RefreshShop();
    }

    public void RefreshShop()
    {
        //vymaze stare sloty v obchode
        foreach (Transform child in container) Destroy(child.gameObject);

        foreach (item_data item in allItemsForSale)
        {
            if (item == null) continue;

            //pokud hrac nema dostatecny level item se nezobrazi
            if (lvl_manager.instance != null)
            {
                if (item.requiredLevel > lvl_manager.instance.unlockedLevel) continue;
            }

            //vytvori slot a nastavi mu data
            GameObject go = Instantiate(slotPrefab, container);
            shop_slot_script slot = go.GetComponent<shop_slot_script>();
            if (slot != null) slot.Setup(item);
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
        //nastavi viditelnost HUD
        manager.instance.HUDVisible(true);
    }

    public bool IsOpen()
    {
        return shopPanel != null && shopPanel.activeSelf;
    }

    public void ForceClose()
    {
        if (IsOpen()) CloseShop();
    }
}