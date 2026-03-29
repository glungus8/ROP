using UnityEngine;
using UnityEngine.AI;

public abstract class enemy_base : MonoBehaviour
{
    public float moveSpeed, hp, damage, attackRange;

    protected Transform player;
    protected NavMeshAgent agent;
    protected SpriteRenderer sprite;
    bool dead = false;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();
        FindPlayer();

        //pridani mod
        if (modifier_manager.instance != null)
        {
            hp *= modifier_manager.instance.enemyHpMul;
            moveSpeed *= modifier_manager.instance.enemySpeedMul;

            //vizualni zmena podle hp
            if (modifier_manager.instance.enemyHpMul > 1.1f)
                sprite.color = new Color(1f, 0.5f, 0.5f); //vic hp- cervena
            else if (modifier_manager.instance.enemyHpMul < 0.9f)
                sprite.color = new Color(0.5f, 1f, 0.5f); //min hp- zelena
        }

        if (agent != null)
        {
            //nastaveni pro 2d pohyb
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            //zapis upravene rychlosti do agenta
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
        if (player == null) return;

        if (player != null && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            agent.speed = moveSpeed;

            //otoceni spritu podle smeru pohybu
            sprite.flipX = player.position.x < transform.position.x;

            float distance = Vector2.Distance(transform.position, player.position);

            //utok pokud je hrac blizko
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
        if (dead) return;

        hp -= amount;

        if (modifier_manager.instance != null && modifier_manager.instance.lifeSteal > 0)
        {
            if (player != null)
            {
                player_manager pm = player.GetComponent<player_manager>();
                if (pm != null) pm.Heal(modifier_manager.instance.lifeSteal);
            }
        }

        if (hp <= 0)
        {
            dead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        if (modifier_manager.instance != null && modifier_manager.instance.enemiesExplode)
        {
            Explode();
        }

        if (wave_manager.instance != null)
        {
            wave_manager.instance.EnemyDied();
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        //najde objekty v okruhu
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (var hit in hitColliders)
        {
            //pokud je v dosahu hrac dostane dmg
            if (hit.CompareTag("Player"))
            {
                player_manager pm = hit.GetComponent<player_manager>();
                if (pm != null)
                {
                    pm.TakeDamage(15f);
                }
            }
        }
    }

    protected abstract void PerformAction();
}