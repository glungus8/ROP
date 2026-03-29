using UnityEngine;

public class Mage : Role
{
    float healTimer = 0, auraDuration = 5f, auraRadius = 5f, nextHealTime = 0;

    public Mage()
    {
        roleName = "Mage";
        maxHP = 80;
        damage = 40;       
        speed = 5f;        
        attackRange = 4.0f; 
        hpRegen = 0.5f;
        ultCooldown = 13f;
        unlockCost = 50;
        isUnlocked = false;
    }

    public override void UseUlt(player_manager player)
    {
        healTimer = auraDuration;

        //zapne vizualni efekt
        if (player.mageAura != null)
        {
            player.mageAura.SetActive(true);
        }
    }

    public override void UpdateRole(player_manager player)
    {
        base.UpdateRole(player);
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;

            //kazdy 0.5s prida 2 HP
            if (Time.time >= nextHealTime)
            {
                player.Heal(2);

                //hleda enemaky v kruhu a da je do pole
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, auraRadius);
                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        enemy_base enHP = enemy.GetComponent<enemy_base>();
                        if (enHP != null) enHP.TakeDamage(10);
                    }
                }

                nextHealTime = Time.time + 0.5f; //nastavi dalsi heal + hit za 0.5s
            }

            //kruh zmizi az vyprsi cas
            if (healTimer <= 0)
            {
                if (player.mageAura != null)
                {
                    player.mageAura.SetActive(false);
                }
            }
        }
    }
}