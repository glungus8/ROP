using UnityEngine;
using UnityEngine.SceneManagement;

public class player_spawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        SceneManager.SetActiveScene(gameObject.scene); //aktualni aktivni scena
        Instantiate(playerPrefab, transform.position, Quaternion.identity); //vytvori prefab tam kde ma byt
    }
}
