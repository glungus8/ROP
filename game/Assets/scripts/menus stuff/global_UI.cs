using UnityEngine;

public class global_UI : MonoBehaviour
{
    static global_UI instance;

    private void Awake()
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
}
