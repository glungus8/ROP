using UnityEngine;

[System.Serializable] //aby to slo videt v inspectoru
public abstract class Role
{
    public string roleName;
    public int maxHP, damage, maxEnergy;
    public float speed, attackRange = 1f, ultCooldown = 10f, energy, hpRegen = 0.5f;
    public bool isUnlocked = false;
    public int unlockCost = 0;

    public virtual int ModifyDamageTaken(int dmg) => dmg; //co se stane pri zasahu

    public virtual void UseUlt(player_manager player) { }

    //bezi kazdy snimek (pro efekty co trvaji nejakou dobu)
    public virtual void UpdateRole(player_manager player)
    {
        if (player.hp < maxHP) //pasivni heal
        {
            player.hp += hpRegen * Time.deltaTime;
            if (player.hp > maxHP) player.hp = maxHP;
        }
    }
}