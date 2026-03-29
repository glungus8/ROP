using UnityEngine;

public class coin_manager : MonoBehaviour
{
    public static coin_manager instance; //staticka instance - pristup odkudkoliv
    public int coins;

    void Awake()
    {
        if (instance == null) //instance bude vzdy jen 1
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //bude v kazdy scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //vezme si coiny ze savu
        if (save_manager.instance != null && save_manager.instance.currentSave != null)
        {
            coins = save_manager.instance.currentSave.coins;
        }
    }

    public void AddCoins(int kolik)
    {
        coins += kolik;
    }
}
