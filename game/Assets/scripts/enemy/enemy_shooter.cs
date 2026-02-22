using UnityEngine;

public class enemy_shooterr : enemy_base
{
    public GameObject arrowPrefab;
    public float fireRate = 2f;
    private float nextFireTime;

    protected override void Start()
    {
        hp = 40f;
        moveSpeed = 4f;
        damage = 4f;
        attackRange = 7f;

        base.Start();
    }

    protected override void PerformAction()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (player == null) return;

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized; //vypocita se smer k hraci

        //natoceni sipu k hraci
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //prida sipu dmg
        enenmy_arrow arrowScript = arrow.GetComponent<enenmy_arrow>();
        if (arrowScript != null) arrowScript.damage = damage;
    }
}
