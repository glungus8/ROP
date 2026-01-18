using UnityEngine;
using TMPro;

public class manager : MonoBehaviour
{
    public TMP_Text coinsText;
    public TMP_Text hpText;
    public TMP_Text energyText;
    public TMP_Text ultText;

    public int coins = 0;

    public float hp = 100f;
    public float hpRegen = 5f;       
    public float regenDelay = 1f;    
    float lastDmgTimer;    


    public float en = 100f;
    public float enMax = 100f;
    public float enCost = 25f;
    public float enRegen = 10f;

    public float ult = 0;
    public float ultMax = 100f;
    public float ultRegen = 5f;

    void Update()
    {
        Energy();
        Ult();
        UpdateUI();
        AddCoins();
        HP();
    }

    void Energy()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (en >= enCost)
                en -= enCost;
        }

        if (en < enMax)
            en += enRegen * Time.deltaTime;

        en = Mathf.Clamp(en, 0, enMax);
    }

    void Ult()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ult == 100) ult = 0;
        }

        if (ult < ultMax)
            ult += ultRegen * Time.deltaTime;

        ult = Mathf.Clamp(ult, 0, ultMax);
    }

    void AddCoins()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            coins += 1;
        }
    }

    void HP()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hp -= 10;
            lastDmgTimer = Time.time;
        }

        if (Time.time - lastDmgTimer >= regenDelay)
        {
            hp += hpRegen * Time.deltaTime;
        }

        hp = Mathf.Clamp(hp, 0, 100);
    }

    void UpdateUI()
    {
        coinsText.text = coins + " g";
        hpText.text = Mathf.RoundToInt(hp) + " HP";
        energyText.text = Mathf.RoundToInt(en) + " EN";
        ultText.text = Mathf.RoundToInt(ult) + " ULT";
    }
}
