using UnityEngine;

public class enemy_base : MonoBehaviour
{
    public float moveSpeed = 2f;
    protected Transform player;

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
            Debug.Log("Enemy naöel hr·Ëe");
        }
        else
        {
            Debug.LogError("Enemy NENAäEL hr·Ëe!");
        }
    }
}
