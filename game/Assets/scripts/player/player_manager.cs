using UnityEngine;

public class player_manager : MonoBehaviour
{
    public Role role;
    public float hp, moveSpeed, ultCd, maxHP;
    public int damage;
    public GameObject mageAura, arrowPrefab, shield;

    public RuntimeAnimatorController archerController;
    public AnimatorOverrideController mageOverride;
    public AnimatorOverrideController tankOverride;

    void Start()
    {
        if (lvl_manager.instance != null && save_manager.instance != null)
        {
            //vezme si ze savu data
            save_manager.instance.PushDataToManagers();
            int index = lvl_manager.instance.roleIndex;

            //da data na roli
            Role roleToApply;
            if (index == 0) roleToApply = new Tank();
            else if (index == 1) roleToApply = new Mage();
            else roleToApply = new Archer();

            SetRole(roleToApply, index);
        }
    }

    void Update()
    {
        if (role != null)
        {
            role.UpdateRole(this);
        } 

        if (ultCd > 0)
        {
            ultCd -= Time.deltaTime;
        }
        else
        {
            ultCd = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (role == null || ultCd > 0) return;

            //dmg za pouziti ult
            if (modifier_manager.instance != null && modifier_manager.instance.upgradeUltCost)
            {
                TakeDamage(30f);
            }

            role.UseUlt(this);
            ultCd = role.ultCooldown; //nastavi cd
        }
    }


    public void SetRole(Role newRole, int index)
    {
        if (newRole == null) return;

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            if (index == 0) anim.runtimeAnimatorController = tankOverride;
            else if (index == 1) anim.runtimeAnimatorController = mageOverride;
            else anim.runtimeAnimatorController = archerController;
        }

        role = newRole;
        if (mageAura != null) mageAura.SetActive(false);
        if (shield != null) shield.SetActive(false);

        //base staty z role
        float baseHp = newRole.maxHP;
        int baseDamage = newRole.damage;
        moveSpeed = newRole.speed;

        //+ bonusy  z vybaveni
        if (player_equipment.instance != null)
        {
            var weapon = player_equipment.instance.equippedWeapon;
            if (weapon != null) baseDamage += (int)weapon.GetCurrentBoost();

            var armor = player_equipment.instance.equippedArmor;
            if (armor != null) baseHp += armor.GetCurrentBoost();

            //sunda zbran co nesedi k roli
            if (weapon != null && weapon.data != null && weapon.data.forRole.ToLower() != "all" &&
            weapon.data.forRole.ToLower() != newRole.GetType().Name.ToLower())
            {
                player_equipment.instance.equippedWeapon = null;
                if (inventory_ui.instance != null) inventory_ui.instance.RefreshInventory();

                //prepocita aby se staty neprenesly do novy role
                SetRole(newRole, index);
                return;
            }
        }

        //update pro hp a maxhp
        if (baseHp > maxHP) hp += (baseHp - maxHP);
        maxHP = baseHp;
        damage = baseDamage;

        if (hp > maxHP) hp = maxHP;
        if (hp <= 0) hp = maxHP;

        if (lvl_manager.instance != null) lvl_manager.instance.roleIndex = index;
    }

    public void TakeDamage(float dmg)
    {
        hp -= role.ModifyDamage((int)dmg);

        if (hp <= 0)
        {
            modifier_manager.instance.ResetModifiers();
            hp = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene("base");
        }
    }

    void OnEnable()
    {
        if (role_manager.instance != null)
        {
            role_manager.instance.pm = this;

            Role currentRole = role_manager.instance.GetRole();

            //pokud ma rm vybranou roli nastavime ji
            if (currentRole != null && lvl_manager.instance != null)
            {
                int savedIndex = lvl_manager.instance.roleIndex;
                SetRole(currentRole, savedIndex);
            }
        }
    }

    public void Heal(float kolik)
    {
        hp += kolik;
        
        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }
}