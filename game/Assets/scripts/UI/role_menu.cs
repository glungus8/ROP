using UnityEngine;
using UnityEngine.SceneManagement;

public class role_menu : MonoBehaviour
{
    public static role_menu instance;
    GameObject roleMenu;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "base")
            return;

        if (lvl_manager.instance != null && lvl_manager.instance.IsOpen()) return;
        if (shop_ui.instance != null && shop_ui.instance.IsOpen()) return;
        if (inventory_ui.instance != null && inventory_ui.instance.IsOpen()) return;
        if (pause_menu.instance != null && pause_menu.instance.pauseMenu.activeSelf) return;

        if (roleMenu == null)
        {
            roleMenu = Find("role_menu");
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleMenu();
        }
    }

    GameObject Find(string name) //najde i neaktivni menu
    {
        GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in all)
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

        if (manager.instance != null)
        {
            manager.instance.HUDVisible(!state);
        }

        Time.timeScale = state ? 0f : 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ForceClose()
    {
        if (roleMenu != null && roleMenu.activeSelf)
        {
            roleMenu.SetActive(false);
            manager.instance.HUDVisible(true);
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public bool IsOpen()
    {
        return roleMenu != null && roleMenu.activeSelf;
    }
}
