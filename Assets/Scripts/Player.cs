using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 4f;            // Player horizontal movement speed

    // jump variables
    public float jumpForce = 8f;            // Vertical jump strength
    public Transform groundCheck;           // Empty object at Player's feet
    public float groundCheckRadius = 0.2f;  // Size of the circle used to detect ground
    public LayerMask groundLayer;           // Which layer the ground is on

    // internal variables
    private Rigidbody2D rb;                 // Reference to player rigidbody
    private bool isGrounded;                // Grounded check

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    // Getting the animator component on the player
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");  // Get the input from keyboard A/D or left/right arrows
        // apply horizontal speed
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        // Jump movement
        if(isGrounded  && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        SetAnimation(moveInput);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
            {
                animator.Play("Player_Fall");
            }
        }
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
