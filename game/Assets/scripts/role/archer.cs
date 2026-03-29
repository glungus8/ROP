using UnityEngine;

public class Archer : Role
{
    float nextAtkTime = 0f, attackCooldown = 0.4f;

    public Archer()
    {
        roleName = "Archer";
        maxHP = 90;
        damage = 25;
        speed = 7f;
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
            if (Time.time >= nextAtkTime) //aby nemohl spamovat atk
            {
                BasicAttack(player);
                nextAtkTime = Time.time + attackCooldown; 
            }
        }
    }

    void BasicAttack(player_manager player)
    {
        SpawnArrow(player, 0f);
    }

    public override void UseUlt(player_manager player)
    {
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
        if (pause_menu.instance != null && pause_menu.instance.pauseMenu.activeSelf) return;

        movement movement = player.GetComponent<movement>();
        Vector2 shootDir = Vector2.right;

        if (movement != null)
        {
            shootDir = movement.GetLastMove(); //aby se vedelo jakym smerem strelit
        }

        //vypocet uhlu Atan2 vezme X Y a spocita uhel od zakladniho smeru (osa X doprava) a Rad2Deg z toho udela stupne
        float baseAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        //Quaternion => rotace, Euler(x,y,z) jak se natoci kolem jaky osy(z), baseAngle kam se hrac diva, angleOffset od ult 
        Quaternion finalRotation = Quaternion.Euler(0, 0, baseAngle + angleOffset);

        GameObject arrow = Object.Instantiate(player.arrowPrefab, player.transform.position, finalRotation);

        Arrow arrowScr = arrow.GetComponent<Arrow>();
        if (arrowScr != null)
        {
            arrowScr.damage = damage;
        }
    }
}