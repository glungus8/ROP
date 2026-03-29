using UnityEngine;

public class role_manager : MonoBehaviour
{
    public static role_manager instance;
    public coin_manager currency;
    public player_manager pm;
    public Role[] roles;

    int selectedIndex = 0;

    void Start()
    {
        pm = FindFirstObjectByType<player_manager>();
    }

    void Awake()
    {
        instance = this;

        //seznam roli
        roles = new Role[] 
        {
        new Tank(),
        new Mage(),
        new Archer()
        };

        //nacte ze savu ktery role ma koupeny a kterou vybranou
        if (save_manager.instance != null && save_manager.instance.currentSave != null)
        {
            int loadedIndex = save_manager.instance.currentSave.roleIndex;

            //opravi se pokud je v savu blbost
            if (loadedIndex < 0 || loadedIndex >= roles.Length)
            {
                loadedIndex = 0;
                save_manager.instance.currentSave.roleIndex = 0;
            }

            LoadUnlockedStates(
                save_manager.instance.currentSave.unlockedRoles,
                loadedIndex
            );
        }
    }

    public bool[] GetUnlockedStates() //vrati jaky role jsou odemceny
    {
        bool[] states = new bool[roles.Length];
        for (int i = 0; i < roles.Length; i++)
        {
            states[i] = roles[i].isUnlocked;
        }
        return states;
    }

    public void LoadUnlockedStates(bool[] states, int lastSelectedIndex) //nacte odemceny role ze savu
    {
        if (states == null || states.Length != roles.Length) return;

        for (int i = 0; i < roles.Length; i++)
        {
            roles[i].isUnlocked = states[i];
        }
        selectedIndex = lastSelectedIndex;
    }

    public Role GetRole() //vrati vybranou roli
    {
        if (roles == null || roles.Length == 0) return null;
        return roles[selectedIndex];
    }

    public void ClickTank() => UseRole(0);
    public void ClickMage() => UseRole(1);
    public void ClickArcher() => UseRole(2);

    void UseRole(int index)
    {
        Role r = roles[index];

        if (!r.isUnlocked)
        {
            if (coin_manager.instance.coins >= r.unlockCost)
            {
                coin_manager.instance.AddCoins(-r.unlockCost);
                r.isUnlocked = true;
            }
            else
            {
                Debug.Log("malo penez");
                return;
            }
        }

        //pokud existuje hrac tak mu zmenime roli
        if (pm != null)
        {
            pm.SetRole(roles[index], index);
        }

        if (lvl_manager.instance != null)
        {
            lvl_manager.instance.roleIndex = index;
        }

        //ulozi hru
        if (save_manager.instance != null)
            save_manager.instance.SaveGame();
    }
}