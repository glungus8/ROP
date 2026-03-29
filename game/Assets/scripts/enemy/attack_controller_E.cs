using UnityEngine;

public class attack_controller_E : MonoBehaviour
{
    public GameObject up, down, left, right;
    public float attackDuration = 0.15f;

    void Start()
    {
        //na zacatku vsechno vypne
        DisableAll();
    }

    public void Attack(Vector2 moveDir)
    {
        //vypne predchozi utoky
        DisableAll();

        //urci smer utoku podle vektoru pohybu
        if (moveDir.y > 0.5f) up.SetActive(true);
        else if (moveDir.y < -0.5f) down.SetActive(true);
        else if (moveDir.x < -0.5f) left.SetActive(true);
        else if (moveDir.x > 0.5f) right.SetActive(true);
        else down.SetActive(true); // zakladni smer pokud se nehýbe

        //za urcity cas vsechno zase vypne
        Invoke(nameof(DisableAll), attackDuration);
    }

    public void DisableAll()
    {
        //proti chybam a vypnuti vsech hitboxu
        if (up) up.SetActive(false);
        if (down) down.SetActive(false);
        if (left) left.SetActive(false);
        if (right) right.SetActive(false);
    }
}