using UnityEngine;

public class lvl_exit : MonoBehaviour
{
    public int level;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //jestli je player v triggeru nastavi se completed lvl
        {
            lvl_manager.instance.CompleteLvl(level);
        }
    }
}
