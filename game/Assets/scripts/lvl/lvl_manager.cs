using UnityEngine;                  
using UnityEngine.SceneManagement;  
using UnityEngine.UI;               

public class lvl_manager : MonoBehaviour
{
    public static lvl_manager instance; //staticka instance - pristup odkudkoliv
    public int unlockedLevel = 1, roleIndex = 0;
    public equipable_item savedWeapon;
    public equipable_item savedArmor;

    GameObject lvlMenu;

    void Awake()
    {
        if (instance == null) //pokud neni lvl_manager
        {
            instance = this; //tahle instance se stane hlavni
            DontDestroyOnLoad(gameObject); //pri zmene sceny se neznici
            SceneManager.sceneLoaded += OnSceneLoaded; //zavola OnSceneLoaded po nacteni sceny
        }
        else
        {
            Destroy(gameObject); //znici duplicity
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //odebere event po zniceni
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lvlMenu = Find("level_menu");

        if (lvlMenu != null)
        {
            lvlMenu.SetActive(false); //menu je po nacteni sceny vzdy zavrene
            Time.timeScale = 1f;
            BtnSetup(); //nastavi tlacitka
        }
    }

    GameObject Find(string name) //najde i neaktivni menu
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.scene.isLoaded) //kontrola jmena + ze patri do aktualni sceny
                return obj;
        }
        return null;
    }

    void BtnSetup()
    {
        if (lvlMenu == null) return;

        Button[] buttons = lvlMenu.GetComponentsInChildren<Button>(true);

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();

            if (btn.name.ToLower().Contains("close"))
            {
                btn.onClick.AddListener(CloseLvlMenu);
            }
            
            //jen pro lvl tlacitka
            else if (btn.name.Contains("level"))
            {
                string nameOnlyNumber = btn.name.Replace("level", "");
                if (int.TryParse(nameOnlyNumber, out int lvlIndex))
                {
                    btn.onClick.AddListener(() => LoadLevel(lvlIndex));
                    btn.interactable = (lvlIndex <= unlockedLevel);
                }
            }
        }
    }

    public void OpenLvlMenu()
    {
        if (pause_menu.instance != null && pause_menu.instance.pauseMenu.activeSelf) return;
        if (inventory_ui.instance != null && inventory_ui.instance.IsOpen()) return;

        if (lvlMenu == null)
        {
            return;
        }

        lvlMenu.SetActive(true);
        Time.timeScale = 0f;
        //nastavi viditelnost HUD
        manager.instance.HUDVisible(false);
    }

    public void CloseLvlMenu()
    {
        if (lvlMenu == null)
        {
            return;
        }

        lvlMenu.SetActive(false);
        Time.timeScale = 1f;
        //nastavi viditelnost HUD
        manager.instance.HUDVisible(true);
    }

    public void LoadLevel(int levelNumber)
    {
        if (modifier_manager.instance != null)
        {
            modifier_manager.instance.ResetModifiers();
        }

        CloseLvlMenu();
        SceneManager.LoadScene("map" + levelNumber);
    }

    public void CompleteLvl(int levelNumber)
    {
        if (wave_manager.instance != null && wave_manager.instance.summaryText != null)
        {
            wave_manager.instance.summaryText.text = "";
        }

        if (levelNumber >= unlockedLevel) // pokud je tento level nejvyssi dosazeny
            unlockedLevel = levelNumber + 1; //odemkne dalsi level

        //ulozi hru
        if (save_manager.instance != null)
            save_manager.instance.SaveGame();

        Time.timeScale = 1f;
        SceneManager.LoadScene("base");
    }

    public bool IsOpen()
    {
        return lvlMenu != null && lvlMenu.activeSelf;
    }
}
