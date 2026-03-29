using UnityEngine;
using System.Collections.Generic;

public class player_equipment : MonoBehaviour
{
    public static player_equipment instance;

    public equipable_item equippedWeapon;
    public equipable_item equippedArmor;
    public List<equipable_item> inventory = new List<equipable_item>();

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void EquipItem(equipable_item item)
    {
        if (item == null || item.data == null)
        {
            return;
        }

        //kontrola role pro zbrane
        if (item.data.type == item_data.ItemType.Weapon)
        {
            string currentRole = GetCurrentRoleName().ToLower().Trim();
            string requiredRole = item.data.forRole.ToLower().Trim();

            if (requiredRole != "all" && requiredRole != currentRole)
            {
                return;
            }
            equippedWeapon = item;
            if (lvl_manager.instance != null) lvl_manager.instance.savedWeapon = item;
        }
        else //pokud je to armor
        {
            equippedArmor = item;
            if (lvl_manager.instance != null) lvl_manager.instance.savedArmor = item;
        }

        UpdatePlayerStats();
        if (inventory_ui.instance != null) inventory_ui.instance.RefreshInventory();
    }

    public void UnequipItem(equipable_item item)
    {
        //pokud sundava zbran
        if (equippedWeapon == item)
        {
            equippedWeapon = null;
            if (lvl_manager.instance != null) lvl_manager.instance.savedWeapon = null;
        }

        //pokud sundava brneni
        if (equippedArmor == item)
        {
            equippedArmor = null;
            if (lvl_manager.instance != null) lvl_manager.instance.savedArmor = null;
        }

        UpdatePlayerStats();
        if (inventory_ui.instance != null) inventory_ui.instance.RefreshInventory();
    }

    public void UpdatePlayerStats()
    {
        //najde hrace a obnovi jeho staty
        player_manager pm = FindFirstObjectByType<player_manager>();
        if (pm != null && pm.role != null && lvl_manager.instance != null)
        {
            pm.SetRole(pm.role, lvl_manager.instance.roleIndex);
        }
        //ulozi hru
        if (save_manager.instance != null)
            save_manager.instance.SaveGame();
    }

    public string GetCurrentRoleName()
    {
        string[] roles = { "tank", "mage", "archer" };
        if (lvl_manager.instance == null) return "all";

        int index = lvl_manager.instance.roleIndex;
        return (index >= 0 && index < roles.Length) ? roles[index] : "all";
    }
}