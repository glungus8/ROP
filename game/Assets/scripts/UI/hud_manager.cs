using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public TMP_Text coinsText, hpText, ultText, infoText;
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
            return;
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

        if (SceneManager.GetActiveScene().name == "base")
        {
            if (save_manager.instance != null && save_manager.instance.currentSave != null)
            {
                int level = save_manager.instance.currentSave.unlockedLevel;
                infoText.text = "BASE LVL: " + level;
            }
        }
    }

    void UpdateUI()
    {
        if (coin_manager.instance != null)
            coinsText.text = coin_manager.instance.coins + " c";

        hpText.text = Mathf.Round(pm.hp) + " / " + Mathf.Round(pm.maxHP);

        if (pm.ultCd > 0)
        {
            ultText.text = "CD: " + Mathf.Ceil(pm.ultCd) + "s";
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