using UnityEngine;
using System.IO;

public class save_manager : MonoBehaviour
{
    public static save_manager instance;
    public data currentSave;
    public int currentSlot = 1;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        if (currentSave == null) return;

        //vezme si data
        if (lvl_manager.instance != null)
        {
            currentSave.unlockedLevel = lvl_manager.instance.unlockedLevel;
            currentSave.roleIndex = lvl_manager.instance.roleIndex;
        }

        if (coin_manager.instance != null)
        {
            currentSave.coins = coin_manager.instance.coins;
        }

        if (role_manager.instance != null)
        {
            currentSave.unlockedRoles = role_manager.instance.GetUnlockedStates();
        }

        if (player_equipment.instance != null)
        {
            currentSave.inventoryItems.Clear();
            var inv = player_equipment.instance.inventory;

            for (int i = 0; i < inv.Count; i++)
            {
                SavedItemData sData = new SavedItemData();
                sData.itemName = inv[i].data.itemName;
                sData.level = inv[i].currentLevel;
                sData.upgradeBonus = inv[i].totalBonusFromUpgrades;
                currentSave.inventoryItems.Add(sData);

                //ulozeni toho co ma hrac na sobe
                if (inv[i] == player_equipment.instance.equippedWeapon) currentSave.equippedWeaponIndex = i;
                if (inv[i] == player_equipment.instance.equippedArmor) currentSave.equippedArmorIndex = i;
            }
        }

        //zapis dat do json souboru
        string json = JsonUtility.ToJson(currentSave);
        File.WriteAllText(GetPath(currentSlot), json);
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        string path = GetPath(slot);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentSave = JsonUtility.FromJson<data>(json);

            //rozeslani dat do manageru
            PushDataToManagers();
        }
    }

    public void PushDataToManagers()
    {
        if (lvl_manager.instance != null)
        {
            if (currentSave.unlockedLevel <= 0) currentSave.unlockedLevel = 1;
            if (currentSave.unlockedLevel >= 4) currentSave.unlockedLevel = 3;

            lvl_manager.instance.unlockedLevel = currentSave.unlockedLevel;
            lvl_manager.instance.roleIndex = currentSave.roleIndex;
        }

        if (coin_manager.instance != null)
        {
            coin_manager.instance.coins = currentSave.coins;
        }

        if (role_manager.instance != null)
        {
            role_manager.instance.LoadUnlockedStates(currentSave.unlockedRoles, currentSave.roleIndex);
        }

        if (player_equipment.instance != null)
        {
            player_equipment.instance.inventory.Clear();
            player_equipment.instance.equippedWeapon = null;
            player_equipment.instance.equippedArmor = null;

            for (int i = 0; i < currentSave.inventoryItems.Count; i++)
            {
                SavedItemData sData = currentSave.inventoryItems[i];

                //nacteni puvodnich dat predmetu z resources
                item_data originalData = Resources.Load<item_data>(sData.itemName);

                if (originalData != null)
                {
                    equipable_item newItem = new equipable_item();
                    newItem.data = originalData;
                    if (sData.level <= 0 || sData.level >= 30) sData.level = 30;
                    newItem.currentLevel = sData.level;
                    newItem.totalBonusFromUpgrades = sData.upgradeBonus;

                    player_equipment.instance.inventory.Add(newItem);

                    //nasazeni veci
                    if (i == currentSave.equippedWeaponIndex) player_equipment.instance.equippedWeapon = newItem;
                    if (i == currentSave.equippedArmorIndex) player_equipment.instance.equippedArmor = newItem;
                }
            }

            //prepocitani statistik postavy a refresh ui
            player_equipment.instance.UpdatePlayerStats();
            inventory_ui.instance.RefreshInventory();
        }
    }

    //reset manageru
    public void ResetManagers()
    {
        if (coin_manager.instance != null) coin_manager.instance.coins = 0;

        if (lvl_manager.instance != null)
        {
            lvl_manager.instance.unlockedLevel = 1;
            lvl_manager.instance.roleIndex = 0;
        }

        if (player_equipment.instance != null)
        {
            player_equipment.instance.inventory.Clear();
            player_equipment.instance.equippedWeapon = null;
            player_equipment.instance.equippedArmor = null;
        }

        if (modifier_manager.instance != null) modifier_manager.instance.ResetModifiers();
    }

    public void DeleteSave(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    string GetPath(int slot)
    {
        return Application.persistentDataPath + "/save_" + slot + ".json";
    }
}