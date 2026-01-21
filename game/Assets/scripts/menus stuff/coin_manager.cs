using UnityEngine;

public class coin_manager : MonoBehaviour
{
    public static coin_manager Instance;
    public int coins = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
