using UnityEngine;

[System.Serializable]
public class equipable_item
{
    public item_data data;
    public int currentLevel = 1;
    public float totalBonusFromUpgrades = 0;

    public float GetCurrentBoost()
    {
        if (data == null) return 0;
        //zakladni boost + bonusy z upgradu
        return data.baseBoost + totalBonusFromUpgrades;
    }

    public int GetUpgradeCost()
    {
        //cena podle levelu
        return Mathf.RoundToInt(data.basePrice * 0.2f * currentLevel);
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        int cost = GetUpgradeCost();
        if (coin_manager.instance != null && coin_manager.instance.coins >= cost)
        {
            coin_manager.instance.AddCoins(-cost);

            //pridani random boostu
            int randomBoost = Random.Range(1, 6);
            totalBonusFromUpgrades += randomBoost;

            currentLevel++;

            //prepocita staty hrace po vylepseni
            if (player_equipment.instance != null)
                player_equipment.instance.UpdatePlayerStats();
        }
    }

    //kontrola podminek
    public bool CanUpgrade()
    {
        if (lvl_manager.instance == null) return false;

        int reached = lvl_manager.instance.unlockedLevel;

        if (currentLevel < 10) return true;
        if (currentLevel < 20) return reached >= 2;
        if (currentLevel < 30) return reached >= 3;
        return reached >= 4;
    }
}