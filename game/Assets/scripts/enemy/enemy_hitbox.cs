using UnityEngine;

public class enemy_hitbox : MonoBehaviour
{
    private enemy_base parentEnemy;

    void Start()
    {
        parentEnemy = GetComponentInParent<enemy_base>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player_manager pm = other.GetComponent<player_manager>();
            if (pm != null && parentEnemy != null)
            {
                pm.TakeDamage(parentEnemy.damage);
                gameObject.SetActive(false); //po 1 hitu hitbox zmizi 
            }
        }
    }
}