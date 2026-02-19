using UnityEngine;

public class pause_menu : MonoBehaviour
{
    public GameObject pauseMenu;
    bool paused = false;

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
            TogglePause();
        }
    }

    public void TogglePause() //prepnuti mezi Pause a Resume
    {
        paused = !paused;

        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    public void ForceClosePause()
    {
        paused = false;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
