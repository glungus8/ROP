using UnityEngine;

public class attack_controller : MonoBehaviour
{
    public GameObject up, down, left, right;

    player_manager pm;
    movement move;

    void Start()
    {
        //najde komponenty hrace
        pm = GetComponentInParent<player_manager>();
        move = GetComponentInParent<movement>();

        DisableAll();
    }

    void Update()
    {
        if (Time.timeScale != 0 && Input.GetMouseButtonDown(0))
        {
            if (pm.role is Archer)
            {
                return;
            }

            Attack();
        }
    }

    void Attack()
    {
        DisableAll();

        Vector2 dir = move.GetLastMove();

        //aktivuje spravny hitbox podle smeru
        if (dir.y > 0) up.SetActive(true);
        else if (dir.y < 0) down.SetActive(true);
        else if (dir.x < 0) left.SetActive(true);
        else if (dir.x > 0) right.SetActive(true);
        else down.SetActive(true);

        //nastavi jak dlouho utok trva
        Invoke(nameof(DisableAll), 0.15f);
    }

    void DisableAll()
    {
        //zabrani chybe kdyby v inspectoru neco nebylo
        if (up) up.SetActive(false);
        if (down) down.SetActive(false);
        if (left) left.SetActive(false);
        if (right) right.SetActive(false);
    }
}