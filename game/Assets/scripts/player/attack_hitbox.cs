using UnityEngine;

public class attack_hitbox : MonoBehaviour
{
    player_manager pm;

    void Start()
    {
        pm = GetComponentInParent<player_manager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        enemy_base enemy = other.GetComponent<enemy_base>();

        if (enemy != null && pm != null)
        {
            enemy.TakeDamage(pm.role.damage); //da enemy dmg
        }
    }
}