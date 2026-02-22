using UnityEngine;

public class EnemyNormal : enemy_base
{
    public float attackRate = 1.5f;
    private float nextAttackTime;
    private attack_controller_E attackController;

    protected override void Start()
    {
        hp = 50f;
        moveSpeed = 4f;
        damage = 5f;

        base.Start();
        attackController = GetComponentInChildren<attack_controller_E>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            agent.isStopped = true;
            if (Time.time >= nextAttackTime)
            {
                PerformAction();
                nextAttackTime = Time.time + attackRate;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            agent.speed = moveSpeed;
        }
    }

    protected override void PerformAction()
    {
        if (attackController != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            attackController.Attack(dir);
        }
    }
}