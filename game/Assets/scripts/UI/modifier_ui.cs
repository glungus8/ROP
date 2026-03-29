using UnityEngine;
using TMPro;
using System.Collections;

public class modifier_ui : MonoBehaviour
{
    public static modifier_ui instance;
    public GameObject panel;

    [Header("texty na tlacitkach")]
    public TMP_Text easyText;
    public TMP_Text hardText;

    string[] easyMods = { "EasyHP", "EasySpeed", "Leech" };
    string[] easyNames = { "- enemy hp", "+ player speed", "attacking heals" };

    string[] hardMods = { "HardHP", "HardSpeed", "Explode", "UltTax" };
    string[] hardNames = { "+ enemy hp", "+ enemy speed", "enemies explode", "ult costs hp" };

    string currentEasy, currentHard;
    private bool canClick = false;

    void Awake()
    {
        instance = this;
    }

    void RollModifiers()
    {
        currentEasy = GetMod(easyMods, easyNames, out string eName);
        easyText.text = eName;

        currentHard = GetMod(hardMods, hardNames, out string hName);
        hardText.text = hName;
    }

    string GetMod(string[] mods, string[] names, out string displayName)
    {
        System.Collections.Generic.List<int> available = new System.Collections.Generic.List<int>();

        for (int i = 0; i < mods.Length; i++)
        {
            if (modifier_manager.instance.IsAvailable(mods[i]))
            {
                available.Add(i);
            }
        }

        if (available.Count == 0)
        {
            displayName = "+5 coins";
            return "CoinBonus";
        }

        //nahodne vybere dostupny mod
        int randomIdx = available[Random.Range(0, available.Count)];
        displayName = names[randomIdx];
        return mods[randomIdx];
    }

    public void OpenMenu()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        RollModifiers();

        //aby neslo na tlacitka kliknout hned
        StartCoroutine(ClickDelay(0.5f));
    }

    IEnumerator ClickDelay(float delay)
    {
        canClick = false;
        yield return new WaitForSecondsRealtime(delay);
        canClick = true;
    }

    public void ClickEasy()
    {
        if (!canClick) return;

        modifier_manager.instance.AddModifier(currentEasy);
        CloseMenu();
    }

    public void ClickHard()
    {
        if (!canClick) return;

        modifier_manager.instance.AddModifier(currentHard);
        CloseMenu();
    }

    public void ClickRoll()
    {
        if (!canClick) return;

        RollModifiers();
    }

    public void CloseMenu()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;

        if (wave_manager.instance != null)
        {
            wave_manager.instance.ModSelected();
        }
    }
}