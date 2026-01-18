using UnityEngine;

public class pause_menu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

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
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause() //pauza hry
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume() //vraceni se zpatky do hry 
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause() //prepnuti mezi Pause a Resume
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }
}
