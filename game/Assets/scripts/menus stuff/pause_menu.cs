using UnityEngine;

public class pause_menu : MonoBehaviour
{
    public static pause_menu instance;
    public GameObject pauseMenu;
    bool paused = false;

    void Awake() => instance = this;

    void Start()
    {
        if (pauseMenu != null) //pause menu skryty na startu
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

            TogglePause();
        }
    }

    public void TogglePause() //prepnuti mezi Pause a Resume
    {
        paused = !paused;

        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;

        //nastavi viditelnost HUD podle toho jestli jsem/nejsem v pause
        manager.instance.HUDVisible(!paused);
    }

    public void ForceClosePause()
    {
        paused = false;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
