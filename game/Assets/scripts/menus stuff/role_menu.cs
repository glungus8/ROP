using UnityEngine;
using UnityEngine.SceneManagement;

public class role_menu : MonoBehaviour
{
    public static role_menu instance;
    GameObject roleMenu;

    void Awake() => instance = this;

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "base")
            return;

        if (lvl_manager.instance != null && lvl_manager.instance.IsOpen()) return;

        if (roleMenu == null)
        {
            roleMenu = Find("role_menu");
            if (roleMenu != null) roleMenu.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleMenu();
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

    public void ToggleMenu()
    {
        if (roleMenu == null) return;

        bool state = !roleMenu.activeSelf;
        roleMenu.SetActive(state);
        manager.instance.HUDVisible(!state); //nastavi viditelnost HUD
        Time.timeScale = state ? 0f : 1f;
    }

    public void ForceClose()
    {
        if (roleMenu != null && roleMenu.activeSelf)
        {
            roleMenu.SetActive(false);
            manager.instance.HUDVisible(true);
            Time.timeScale = 1f;
        }
    }

    public bool IsOpen() => roleMenu != null && roleMenu.activeSelf;
}
