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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Aktuální rychlost v movementu: " + pm.role.speed);
        }
    }

    void FixedUpdate()
    {
        float speed = pm.moveSpeed;

        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

        // TEST: Jednou za èas vypíše, kdo se hýbe
        if (moveInput.magnitude > 0 && Time.frameCount % 100 == 0)
        {
            Debug.Log($"Objekt {gameObject.name} se hybe rychlosti {speed} (Role: {pm.role.roleName})");
        }
    }

    public Vector2 GetLastMove()
    {
        return lastMove;
    }

}

