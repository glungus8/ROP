using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private EnemyNormal parentEnemy;

    void Start()
    {
        parentEnemy = GetComponentInParent<EnemyNormal>();
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