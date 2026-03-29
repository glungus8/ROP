using UnityEngine;

public class lvl_exit : MonoBehaviour 
{
    public int level;
    private bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !used) //jestli je player v triggeru nastavi se completed lvl
        {
            used = true;
            int kolik = wave_manager.instance.GetReward();
            coin_manager.instance.AddCoins(kolik);
            Debug.Log("Pridano coinu: " + kolik);
            modifier_manager.instance.ResetModifiers();

            lvl_manager.instance.CompleteLvl(level);
        }
    }
}