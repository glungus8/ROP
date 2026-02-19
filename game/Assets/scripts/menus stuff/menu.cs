using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("base", LoadSceneMode.Single);
    }

    public void OpenSettings()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //aby to fungovalo i v editoru
#endif
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;

        pause_menu pm = FindFirstObjectByType<pause_menu>();
        if (pm != null)
        {
            pm.ForceClosePause(); //aby pause nezustal potom co odejdu do menu
        }

        SceneManager.LoadScene("menu");
    }
}
