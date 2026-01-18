using UnityEngine;                  
using UnityEngine.SceneManagement;  
using UnityEngine.UI;               

public class lvl_manager : MonoBehaviour
{
    public static lvl_manager Instance; //staticka instance - pristup odkudkoliv
    public int unlockedLevel = 1;
    GameObject levelMenu;

    void Awake()
    {
        if (Instance == null) //pokud neni lvl_manager
        {
            Instance = this; //tahle instance se stane hlavni
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
        levelMenu = FindInactiveObjectByName("LevelMenu"); //najde i neaktivni menu

        if (levelMenu != null)
        {
            levelMenu.SetActive(false); //menu je po nacteni sceny vzdy zavrene
            Time.timeScale = 1f;
            SetupButtons(); //nastavi tlacitka
        }
    }

    GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.scene.isLoaded) //kontrola jmena + ze patri do aktualni sceny
                return obj;
        }
        return null;
    }

    void SetupButtons()
    {
        if (levelMenu == null)
            return;

        Button[] buttons = levelMenu.GetComponentsInChildren<Button>(true); //najde vsechny tlacitka v menu

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners(); //odstrani stary listenery

            if (btn.name == "Close")
            {
                btn.onClick.AddListener(CloseLevelMenu);
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

    public void OpenLevelMenu()
    {
        if (levelMenu == null)
        {
            return;
        }

        levelMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseLevelMenu()
    {
        if (levelMenu == null)
            return;

        levelMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadLevel(int levelNumber)
    {
        CloseLevelMenu();
        SceneManager.LoadScene("map" + levelNumber);
    }

    public void CompleteLevel(int levelNumber)
    {
        if (levelNumber >= unlockedLevel) // pokud je tento level nejvyssi dosazeny
            unlockedLevel = levelNumber + 1; //odemkne dalsi level

        Time.timeScale = 1f;
        SceneManager.LoadScene("base");
    }
}
