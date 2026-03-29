using UnityEngine;

public class shop : MonoBehaviour
{
    bool playerInside = false; //je hrac v portalu

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E)) //jestli je v portalu a zmackl E tak se otevre level menu
        {
            shop_ui.instance.OpenShop();
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
