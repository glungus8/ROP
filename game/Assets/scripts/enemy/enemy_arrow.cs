using UnityEngine;

public class enenmy_arrow : MonoBehaviour
{
    public float speed = 8f; 
    public float damage;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_manager playerHealth = collision.GetComponent<player_manager>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
