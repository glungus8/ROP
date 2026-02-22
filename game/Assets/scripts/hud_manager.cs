using UnityEngine;
using TMPro;

public class manager : MonoBehaviour
{
    public TMP_Text coinsText, hpText, energyText, ultText;
    public GameObject HUD;
    public static manager instance;
    player_manager pm;

    private void Awake()
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

    void Update()
    {
        //pm se hleda v kazdem snimku
        if (pm == null)
        {
            pm = FindFirstObjectByType<player_manager>();
            return;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coin_manager.instance != null)
            coinsText.text = coin_manager.instance.coins + " g";

        hpText.text = Mathf.RoundToInt(pm.hp) + " / " + pm.role.maxHP + " HP";

        energyText.text = Mathf.RoundToInt(pm.role.energy) + " EN";

        if (pm.ultCdTimer > 0)
        {
            ultText.text = "CD: " + Mathf.Ceil(pm.ultCdTimer) + "s";
            ultText.color = Color.red;
        }
        else
        {
            ultText.text = "READY";
            ultText.color = Color.yellow;
        }
    }

    public void HUDVisible(bool visible)
    {
        if (HUD != null)
            HUD.SetActive(visible);
    }
}