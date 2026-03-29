using UnityEngine;

public class pause_menu : MonoBehaviour
{
    public static pause_menu instance;
    public GameObject pauseMenu;
    bool paused = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (modifier_ui.instance != null && modifier_ui.instance.panel.activeSelf)
            {
                return; //nepusti se pause
            }

            //zavre vsechny jiny menu
            if (lvl_manager.instance != null && lvl_manager.instance.IsOpen())
            {
                lvl_manager.instance.CloseLvlMenu();
                return;
            }

            if (role_menu.instance != null && role_menu.instance.IsOpen())
            {
                role_menu.instance.ForceClose();
                return;
            }

            if (shop_ui.instance != null && shop_ui.instance.IsOpen())
            {
                shop_ui.instance.ForceClose();
                return;
            }

            if (inventory_ui.instance != null && inventory_ui.instance.IsOpen())
            {
                inventory_ui.instance.ForceClose();
                return;
            }

            TogglePause();
        }
    }

    public void TogglePause() //prepnuti mezi Pause a Resume
    {
        if (pauseMenu == null) return;
        paused = !paused;

        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;

        //nastavi viditelnost HUD podle toho jestli jsem/nejsem v pause
        if (manager.instance != null)
        {
            manager.instance.HUDVisible(!paused);
        }

        //viditelnost cursoru
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ForceClosePause()
    {
        paused = false;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
