using UnityEngine;
using UnityEngine.AI;

public abstract class enemy_base : MonoBehaviour
{
    public float moveSpeed, hp, damage, attackRange;

    protected Transform player;
    protected NavMeshAgent agent;
    protected SpriteRenderer sprite;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();
        FindPlayer();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            agent.speed = moveSpeed;
            agent.acceleration = 10000f; 
            agent.angularSpeed = 10000f;
            agent.stoppingDistance = 0f;
            agent.autoBraking = false;
        }
    }

    protected void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    protected virtual void Update()
    {
        if (player == null) FindPlayer();

        if (player != null && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            agent.speed = moveSpeed;

            if (player.position.x < transform.position.x) sprite.flipX = true;
            else sprite.flipX = false;

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                PerformAction();
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (wave_manager.instance != null)
        {
            wave_manager.instance.EnemyDied();
        }
        Destroy(gameObject);
    }
    protected abstract void PerformAction();
}