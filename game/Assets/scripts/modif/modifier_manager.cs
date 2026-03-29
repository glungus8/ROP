using UnityEngine;

public class modifier_manager : MonoBehaviour
{
    public static modifier_manager instance;

    [Header("mult (1 = zaklad)")]
    public float enemyHpMul = 1f;
    public float enemySpeedMul = 1f;
    public float playerSpeedMul = 1f;
    public float rewardMultiplier = 1f;

    [Header("specialni")]
    public bool upgradeUltCost = false;
    public bool enemiesExplode = false;
    public int lifeSteal = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddModifier(string type)
    {
        //switch se podiva na hodnotu v 'type' a skoci na spravny radek
        switch (type)
        {
            case "HardHP":
                enemyHpMul += 0.3f;
                rewardMultiplier += 0.2f;
                break;
            case "EasyHP":
                enemyHpMul = Mathf.Max(0.2f, enemyHpMul - 0.2f);
                rewardMultiplier -= 0.15f;
                break;
            case "HardSpeed":
                enemySpeedMul += 0.2f;
                rewardMultiplier += 0.15f;
                break;
            case "EasySpeed":
                playerSpeedMul = Mathf.Min(2.0f, playerSpeedMul + 0.2f);
                rewardMultiplier -= 0.15f;
                break;
            case "Leech":
                lifeSteal += 2;
                rewardMultiplier -= 0.2f;
                break;
            case "Explode":
                enemiesExplode = true;
                rewardMultiplier += 0.3f;
                break;
            case "UltTax":
                upgradeUltCost = true;
                rewardMultiplier += 0.4f;
                break;
            case "CoinBonus":
                if (coin_manager.instance != null) coin_manager.instance.AddCoins(5);
                break;
            default:
                break;
        }

        Debug.Log("aktualni reward multiplier: " + rewardMultiplier);
    }

    public bool IsAvailable(string type)
    {
        if (type == "Leech" && lifeSteal >= 6) return false; //limit pro leech
        if (type == "Explode" && enemiesExplode) return false; //jen jednou
        if (type == "UltTax" && upgradeUltCost) return false; //jen jednou

        if (type == "EasyHP" && enemyHpMul <= 0.2f) return false; //limit pro oslabeni enemy
        if (type == "EasySpeed" && playerSpeedMul >= 2.0f) return false; //limit pro rychlost hrace

        return true;
    }

    public void ResetModifiers()
    {
        enemyHpMul = 1f;
        enemySpeedMul = 1f;
        playerSpeedMul = 1f;
        rewardMultiplier = 1f;
        enemiesExplode = false;
        upgradeUltCost = false;
        lifeSteal = 0;
    }
}