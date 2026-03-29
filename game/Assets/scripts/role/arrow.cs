using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 15f;
    public int damage;

    void Start()
    {
        Destroy(gameObject, 3f); //za 3s se sip znici pokud nic netrefil
    }
    void Update()
    {
        //leti ve smeru kam je objekt natoceny (diky baseAngle v archer) Vector2.right je to kam miri spicka sipu
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy_base en = collision.GetComponent<enemy_base>();
            if (en != null) en.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}