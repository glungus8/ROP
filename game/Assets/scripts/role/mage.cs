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
        maxEnergy = 150;   
        energy = 150;
        attackRange = 4.0f; 
        hpRegen = 0.5f;
        ultCooldown = 15f;
        unlockCost = 50;
        isUnlocked = false;
    }

    public override void UseUlt(player_manager player)
    {
        healTimer = auraDuration;

        //zapne vizualni efekt
        if (player.mageAuraVisual != null)
        {
            player.mageAuraVisual.SetActive(true);
        }

        Debug.Log("Mage: AURA AKTIVOVANA!");
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
                nextHealTime = Time.time + 0.5f; //nastavi dalsi heal za 0.5s
            }

            //hleda enemaky v kruhu
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, auraRadius);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    health enHealth = enemy.GetComponent<health>();
                    if (enHealth != null)
                    {
                        enHealth.TakeDamage(1); //da dmg kazdych par snimku
                    }
                }
            }

            //kruh zmizi az vyprsi cas
            if (healTimer <= 0)
            {
                if (player.mageAuraVisual != null)
                {
                    player.mageAuraVisual.SetActive(false);
                }
            }
        }
    }
}