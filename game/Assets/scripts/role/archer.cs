using UnityEngine;

public class Archer : Role
{
    float nextAttackTime = 0f;
    float attackCooldown = 0.4f;

    public Archer()
    {
        roleName = "Archer";
        maxHP = 90;
        damage = 25;
        speed = 7f;
        maxEnergy = 100;
        hpRegen = 0.8f;
        ultCooldown = 10f;
        unlockCost = 30;
        isUnlocked = false;
    }

    public override void UpdateRole(player_manager player)
    {
        base.UpdateRole(player);

        if (Input.GetMouseButton(0)) 
        {
            if (Time.time >= nextAttackTime)
            {
                BasicAttack(player);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void BasicAttack(player_manager player)
    {
        Debug.Log("Archer: vystrel!");
        SpawnArrow(player, 0f);
    }

    public override void UseUlt(player_manager player)
    {
        Debug.Log("Archer ult: MULTI SHOT!");
        float startAngle = -30f;
        float angleStep = 15f;

        for (int i = 0; i < 5; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            SpawnArrow(player, currentAngle);
        }
    }

    void SpawnArrow(player_manager player, float angleOffset)
    {
        if (player.arrowPrefab == null) return;

        movement movement = player.GetComponent<movement>();
        Vector2 shootDir = Vector2.right;

        if (movement != null)
        {
            shootDir = movement.GetLastMove(); //aby se vedelo jakym smerem strelit
        }

        float baseAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg; //vypocet uhlu
        Quaternion finalRotation = Quaternion.Euler(0, 0, baseAngle + angleOffset);

        GameObject arrow = Object.Instantiate(player.arrowPrefab, player.transform.position, finalRotation);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = damage;
        }
    }
}