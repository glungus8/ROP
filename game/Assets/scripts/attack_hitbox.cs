using UnityEngine;

public class attack_hitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        health hp = other.GetComponent<health>(); //vezme si hp enemaka

        if (hp != null)
        {
            hp.TakeDamage(damage);
        }
    }
}
