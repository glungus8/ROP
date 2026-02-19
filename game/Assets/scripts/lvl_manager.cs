using UnityEngine;                  
using UnityEngine.SceneManagement;  
using UnityEngine.UI;               

public class lvl_manager : MonoBehaviour
{
    public static lvl_manager instance; //staticka instance - pristup odkudkoliv
    public int unlockedLevel = 1, roleIndex = 0;
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
        lvlMenu = Find("LevelMenu"); 

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
        if (lvlMenu == null)
            return;

        Button[] buttons = lvlMenu.GetComponentsInChildren<Button>(true); //najde vsechny tlacitka v menu

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners(); //odstrani stary listenery

            if (btn.name == "Close")
            {
                btn.onClick.AddListener(CloseLvlMenu);
            }
            else if (btn.name == "Level 1")
            {
                btn.onClick.AddListener(() => LoadLevel(1));
            }
            else if (btn.name == "Level 2")
            {
                btn.onClick.AddListener(() => LoadLevel(2));
                btn.interactable = unlockedLevel >= 2;
            }
        }
    }

    public void OpenLvlMenu()
    {
        if (lvlMenu == null)
        {
            return;
        }

        lvlMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseLvlMenu()
    {
        if (lvlMenu == null)
            return;

        lvlMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadLevel(int levelNumber)
    {
        CloseLvlMenu();
        SceneManager.LoadScene("map" + levelNumber);
    }

    public void CompleteLvl(int levelNumber)
    {
        coin_manager.instance.AddCoins(10);

        if (levelNumber >= unlockedLevel) // pokud je tento level nejvyssi dosazeny
            unlockedLevel = levelNumber + 1; //odemkne dalsi level

        Time.timeScale = 1f;
        SceneManager.LoadScene("base");
    }
}
