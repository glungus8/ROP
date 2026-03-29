using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject menu_panel;
    public GameObject slots;
    public void StartGame()
    {
        menu_panel.SetActive(false);
        slots.SetActive(true);
    }

    public void OpenSettings()
    {
        
    }

    public void QuitGame()
    {
        if (SceneManager.GetActiveScene().name == "base")
        {
            if (save_manager.instance != null)
            {
                save_manager.instance.SaveGame();
            }
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //aby to fungovalo i v editoru
#endif
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;

        if (pause_menu.instance != null)
        {
            pause_menu.instance.ForceClosePause();
        }

        if (modifier_manager.instance != null)
        {
            modifier_manager.instance.ResetModifiers();
        }

        SceneManager.LoadScene("menu");
    }
}
