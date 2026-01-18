using UnityEngine;
using System.Collections;

public class player_attack : MonoBehaviour
{
    public GameObject hitbox;
    public float attackDuration = 0.2f;
    public float attackCooldown = 0.4f;
    public float attackRange = 0.6f;

    bool canAttack = true;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.timeScale == 0) //neutoci kdyz je hra zastavena
            return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() //korutina (muze se na cas prerusit (yield return) a pak pokracovat)
    {
        canAttack = false; //nemuze utocit dokud neni cd

        //ziska posledni smer z animatoru
        float x = anim.GetFloat("LastMoveX");
        float y = anim.GetFloat("LastMoveY");

        Vector2 dir = new Vector2(x, y).normalized; //smer utoku

        if (dir == Vector2.zero) //pokud hrac nemel pohyb utok bude dolu
            dir = Vector2.down;

        hitbox.transform.localPosition = dir * attackRange; //nastavi hitbox pred hrace podle smeru a vzdalenosti attackRange

        hitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration); //pocka nez hitbox zmizi
        hitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown); //ceka nez muze hrac zase utocit
        canAttack = true;
    }
}
