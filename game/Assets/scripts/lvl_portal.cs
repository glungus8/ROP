using UnityEngine;

public class lvl_portal : MonoBehaviour
{
    private bool playerInside = false; //je hrac v portalu

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E)) //jestli je v portalu a zmackl E tak se otevre level menu
        {
            lvl_manager.Instance.OpenLevelMenu();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //je player v triggeru
        {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
