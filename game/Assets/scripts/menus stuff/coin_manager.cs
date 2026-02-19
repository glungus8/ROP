using UnityEngine;

public class coin_manager : MonoBehaviour
{
    public static coin_manager instance; //staticka instance - pristup odkudkoliv
    public int coins = 0;

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

    public void AddCoins(int kolik)
    {
        coins += kolik;
    }
}
