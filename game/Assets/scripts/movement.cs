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

        anim.SetBool("moving", moveInput != Vector2.zero);
        if (moveInput.x > 0) anim.SetInteger("direction", 3);     // right
        else if (moveInput.x < 0) anim.SetInteger("direction", 2); // left
        else if (moveInput.y > 0) anim.SetInteger("direction", 1); // up
        else if (moveInput.y < 0) anim.SetInteger("direction", 0); // down
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}

