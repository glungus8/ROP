using UnityEngine;

public class movement : MonoBehaviour
{
    player_manager pm; //speed
    Rigidbody2D rb;
    Vector2 moveInput;
    Vector2 lastMove = Vector2.down;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pm = GetComponent<player_manager>();
    }

    void Update()
    {
        //nacte vstup WASD/sipky
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); //diagonalni pohyb nebude rychlejsi

        //animace
        anim.SetFloat("X", moveInput.x);
        anim.SetFloat("Y", moveInput.y);

        bool moving = moveInput.x != 0 || moveInput.y != 0;
        anim.SetBool("moving", moving);

        if (moving)
        {
            anim.SetFloat("LastMoveX", moveInput.x);
            anim.SetFloat("LastMoveY", moveInput.y);
            lastMove = moveInput;

        }
        else
        {
            anim.SetFloat("X", anim.GetFloat("LastMoveX"));
            anim.SetFloat("Y", anim.GetFloat("LastMoveY"));
        }
    }

    void FixedUpdate()
    {
        float speed = pm.moveSpeed;

        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    public Vector2 GetLastMove()
    {
        return lastMove;
    }

}

