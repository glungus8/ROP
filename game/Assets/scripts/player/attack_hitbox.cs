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
        health enemyHealth = other.GetComponent<health>();

        if (enemyHealth != null && pm != null)
        {
            // Vezme poškození z aktuální role hráèe
            enemyHealth.TakeDamage(pm.role.damage);
            Debug.Log("dmg: " + pm.role.damage);
        }
    }
}