using UnityEngine;

public class Tank : Role
{
    float shieldTimer = 0;

    public Tank()
    {
        roleName = "Tank";
        maxHP = 200;
        damage = 15;
        speed = 4f;
        unlockCost = 0;
        attackRange = 1.2f;
        ultCooldown = 10f;
        hpRegen = 2.0f;
        isUnlocked = true;
    }

    public override int ModifyDamage(int dmg)
    {
        if (shieldTimer > 0) return 0; //aktivni stit
        return Mathf.RoundToInt(dmg * 0.7f); //pasivni redukce dmg 30%
    }

    public override void UseUlt(player_manager player)
    {
        shieldTimer = 5f; //zapne stit na 5s

        //zapne vizualni efekt
        if (player.shield != null)
        {
            player.shield.SetActive(true);
        }
    }

    public override void UpdateRole(player_manager player)
    {
        base.UpdateRole(player);
        if (shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
        }

        //vypne vizualni efekt
        if (shieldTimer <= 0)
        {
            if (player.shield != null)
            {
                player.shield.SetActive(false);
            }
        }
    }
}