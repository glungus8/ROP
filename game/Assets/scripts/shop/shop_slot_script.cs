using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class shop_slot_script : MonoBehaviour
{
    public Image iconDisplay;
    public item_data item;
    public TMP_Text nameText, priceText;
    public Button buyButton;

    public void Setup(item_data newItem)
    {
        item = newItem;
        nameText.text = item.itemName;
        priceText.text = item.basePrice + " g";

        if (iconDisplay != null) iconDisplay.sprite = item.itemIcon;

        check_availability();
    }

    void check_availability()
    {
        if (coin_manager.instance == null || buyButton == null) return;

        bool canAfford = coin_manager.instance.coins >= item.basePrice;
        buyButton.interactable = canAfford;
    }

    public void OnBuyClicked()
    {
        if (coin_manager.instance == null || player_equipment.instance == null) return;

        if (coin_manager.instance.coins >= item.basePrice)
        {
            coin_manager.instance.AddCoins(-item.basePrice);

            //instance predmetu pro inv
            equipable_item newItem = new equipable_item();
            newItem.data = this.item;
            newItem.currentLevel = 1;

            player_equipment.instance.inventory.Add(newItem);

            if (shop_ui.instance != null) shop_ui.instance.RefreshShop();

            //ulozi hru
            if (save_manager.instance != null)
                save_manager.instance.SaveGame();
        }
    }
}