using UnityEngine;

public class player_manager : MonoBehaviour
{
    public Role role;
    public float hp, moveSpeed, ultCdTimer;
    public int damage;
    public GameObject mageAuraVisual, arrowPrefab;

    void Start()
    {
        if (role == null)
            SetRole(new Tank());

        //nacteni role z lvl_manager
        if (lvl_manager.instance != null)
        {
            int savedRole = lvl_manager.instance.roleIndex;
            ApplyRoleByIndex(savedRole);
        }
    }

    void Update()
    {
        role?.UpdateRole(this);

        if (ultCdTimer > 0)
        {
            ultCdTimer -= Time.deltaTime;
        }
        else
        {
            ultCdTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            TryUseUlt();
        }
    }

    void TryUseUlt()
    {
        if (role == null) return;

        if (ultCdTimer > 0)
        {
            return;
        }

        role.UseUlt(this);

        ultCdTimer = role.ultCooldown;
    }

    public void SetRole(Role newRole)
    {
        role = newRole;

        if (mageAuraVisual != null) mageAuraVisual.SetActive(false);

        //prepise hodnoty
        moveSpeed = newRole.speed;
        damage = newRole.damage;
        hp = newRole.maxHP;
    }

    public void TakeDamage(float dmg)
    {
        hp -= role.ModifyDamageTaken((int)dmg);

        if (hp <= 0)
        {
            hp = 0;
            Debug.Log("DEAD");
        }
    }

    void OnEnable()
    {
        role_manager rm = FindFirstObjectByType<role_manager>();
        if (rm != null)
        {
            rm.pm = this;
            //pokud ma rm vybranou roli nastavime ji
            if (rm.GetRole() != null)
            {
                SetRole(rm.GetRole());
            }
        }
    }

    public void Heal(float amount)
    {
        hp += amount;
        if (hp > role.maxHP)
        {
            hp = role.maxHP;
        }
    }

    public void ApplyRoleByIndex(int index)
    {
        if (index == 0) SetRole(new Tank());
        else if (index == 1) SetRole(new Mage());
        else if (index == 2) SetRole(new Archer());

        //ulozi se to do manageru aby se to vedelo i v levelu
        if (lvl_manager.instance != null)
        {
            lvl_manager.instance.roleIndex = index;
        }
    }
}