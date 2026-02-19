using UnityEngine;
using UnityEngine.SceneManagement;

public class role_menu : MonoBehaviour
{
    GameObject roleMenu;

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "base")
            return;

        if (roleMenu == null)
        {
            roleMenu = Find("role_menu");
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            roleMenu.SetActive(!roleMenu.activeSelf);
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
}
