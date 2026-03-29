using UnityEngine;

public class enemy_normal : enemy_base
{
    public float attackRate = 1.5f;

    float nextAttackTime;
    attack_controller_E attackController;

    protected override void Start()
    {
        hp = 50f;
        moveSpeed = 4f;
        damage = 5f;

        base.Start();
        attackController = GetComponentInChildren<attack_controller_E>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void PerformAction()
    {
        if (Time.time >= nextAttackTime)
        {
            if (attackController != null)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                attackController.Attack(dir);
            }
            nextAttackTime = Time.time + attackRate;
        }
    }
}