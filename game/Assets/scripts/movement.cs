using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 5f; //speed
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        }
        else
        {
            anim.SetFloat("X", anim.GetFloat("LastMoveX"));
            anim.SetFloat("Y", anim.GetFloat("LastMoveY"));
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}

