using UnityEngine;

public class attack_controller : MonoBehaviour
{
    public GameObject up, down, left, right;
    public float baseAtkRate = 0.5f;
    public Sprite attackSprite;

    float nextAtkTime;
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
        //kontrola cd + kliknuti a jestli neni pause
        if (Time.timeScale != 0 && Input.GetMouseButtonDown(0) && Time.time >= nextAtkTime)
        {
            if (pm.role is Archer) return;

            Attack();

            nextAtkTime = Time.time + baseAtkRate;
        }
    }

    void Attack()
    {
        DisableAll();

        Vector2 dir = move.GetLastMove();
        GameObject activeBox = null;

        //aktivuje spravny hitbox podle smeru
        if (dir.y > 0) up.SetActive(true);
        else if (dir.y < 0) down.SetActive(true);
        else if (dir.x < 0) left.SetActive(true);
        else if (dir.x > 0) right.SetActive(true);
        else down.SetActive(true);

        if (activeBox != null)
        {
            // Najdeme SpriteRenderer na tom konkrétním hitboxu
            SpriteRenderer sr = activeBox.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = attackSprite; // Nastavíme obrázek podle role
            }
            activeBox.SetActive(true);
        }

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