using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SavedItemData
{
    public string itemName;
    public int level;
    public float upgradeBonus;
}
public class data
{
    public string saveName;
    public int unlockedLevel = 1;
    public int coins = 0;
    public int roleIndex = 0;
    public bool[] unlockedRoles = new bool[] { true, false, false };
    public List<SavedItemData> inventoryItems = new List<SavedItemData>();
    public int equippedWeaponIndex = -1;
    public int equippedArmorIndex = -1;
}