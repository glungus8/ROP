using UnityEngine;

public class role_manager : MonoBehaviour
{
    public coin_manager currency;
    public player_manager pm;

    Role[] roles;
    int selectedIndex = 0;

    void Start()
    {
        pm = FindFirstObjectByType<player_manager>();
    }

    void Awake()
    {
        roles = new Role[]
        {
        new Tank(),
        new Mage(),
        new Archer()
        };
    }

    public Role GetRole()
    {
        if (roles == null || roles.Length == 0) return null;
        return roles[selectedIndex];
    }

    public void ClickTank() => TryUseRole(0);
    public void ClickMage() => TryUseRole(1);
    public void ClickArcher() => TryUseRole(2);

    void TryUseRole(int index)
    {
        selectedIndex = index;
        Role r = roles[index];

        //pokud existuje hrac tak mu zmenime roli
        if (pm != null)
        {
            if (r.isUnlocked)
            {
                pm.SetRole(r);
            }
            else if (currency.coins >= r.unlockCost)
            {
                currency.coins -= r.unlockCost;
                r.isUnlocked = true;
                pm.SetRole(r);
            }
            else
            {
                Debug.Log("malo penez");
            }

            //aby si to pamatovalo i v jiny scene
            lvl_manager.instance.roleIndex = index;

            //zmena role
            pm.ApplyRoleByIndex(index);
        }
    }
}