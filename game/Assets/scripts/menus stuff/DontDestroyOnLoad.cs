using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad instance;

    private void Awake()
    {
        if (instance != null && instance != this) //pokud instance existuje tuhle znici
        {
            Destroy(gameObject);
            return;
        }

        instance = this; //nastavi instanci
        DontDestroyOnLoad(gameObject); //zustane mezi scenami
    }
}
