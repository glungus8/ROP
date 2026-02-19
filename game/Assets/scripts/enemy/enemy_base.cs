using UnityEngine;

public class enemy_base : MonoBehaviour
{
    public float moveSpeed = 2f;

    protected Transform player;

    protected virtual void Update()
    {
        //enemy si najde hrace
        if (player == null) 
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
        }
    }
}
