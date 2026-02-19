using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 15f;
    public int damage;
    Vector2 direction = Vector2.right;

    public void SetDirection(Vector3 dir)
    {
        //nastavi smeri a otoci sip vizualne k cili
        direction = dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        //leti ve smeru kam je objekt natoceny (diky baseAngle v archer)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            health en = collision.GetComponent<health>();
            if (en != null) en.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
            Debug.Log("zed");
        }
    }
}