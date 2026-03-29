using UnityEngine;

[CreateAssetMenu(fileName = "item_data", menuName = "Scriptable Objects/item_data")]
public class item_data : ScriptableObject
{
    public string itemName;
    public enum ItemType { Weapon, Armor }
    public ItemType type;
    public Sprite itemIcon;
    public int basePrice;

    [Header("stats boost")]
    public float baseBoost;
    public float upgradeMultiplier = 1.2f;

    [Header("requirements")]
    public int requiredLevel = 1;
    public string forRole = "All";
}