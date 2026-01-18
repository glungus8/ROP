using UnityEngine;

public class health : MonoBehaviour
{
    public int maxHP = 3;
    int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int kolik)
    {
        currentHP -= kolik;
        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
