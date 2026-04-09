using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int coins;
    public int health = 100;

    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 4f;            // Player horizontal movement speed

    // jump variables
    public float jumpForce = 8.0f;            // Vertical jump strength 

    public float jumpContinuesForce = 1.0f;//episode 22

    public Transform groundCheck;           // Empty object at Player's feet
    public float groundCheckRadius = 0.2f;  // Size of the circle used to detect ground
    public LayerMask groundLayer;           // Which layer the ground is on

    public Image healthImage;

    public AudioClip jumpClip;
    public AudioClip hurtClip;

    // internal variables
    private Rigidbody2D rb;                 // Reference to player rigidbody
    private bool isGrounded;                // Grounded check

    private Animator animator;

    private AudioSource audioSource;

    public int extraJumpsValue = 1;
    public int extraJumps;
    
    public float coyoteTime = 0.2f;//added from episode 20
    private float coyoteTimeCounter;//added from episode 20

    public float jumpBufferTime = 0.15f;//added from episode 21
    private float jumpBufferCounter;//added from episode 21


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    // Getting the animator component on the player

        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        extraJumps = extraJumpsValue;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;//episode 20
            extraJumps = extraJumpsValue;
        }
        else 
        {
            coyoteTimeCounter -=Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))//jump buffer time
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f) 
        {
            if (coyoteTimeCounter > 0f) 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                PlaySFX(jumpClip);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                PlaySFX(jumpClip);
                jumpBufferCounter = 0f;
            }
        }
        //added in episode 22 Variable jump height, not working in game, no idea why not
        //no idea why the vid asks for rb.linearVelocityY instead of rb.linearVelocity.y
        // doesn't matter anyways 'cause both ways do not cause you to jump higher when space bar is held
        if (Input.GetButtonDown("Jump") && rb.linearVelocityY > 0)
        {
            rb.AddForceY(jumpContinuesForce);
        }

        SetAnimation(moveInput);

        healthImage.fillAmount = health / 100f;

        //episode 23 Gravity Scaling
        //i do not see the effects of gravity scaling as desribed/shown in the vid
        //can see it now but had to mod the PlayerObject in unity to have a gravity scale of 2
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = 3f;
        }
        else 
        {
            rb.gravityScale = 2f;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            PlaySFX(hurtClip);
            health -= 25;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            StartCoroutine(BlinkRed());

            if (health <= 0)
            {
                Die();
            }
        }
        else if (collision.gameObject.tag == "BouncePad")
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 1.5f);

        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void PlaySFX(AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    //added from episode 18
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Strawberry") 
        {
            extraJumps = 2;
            Destroy(collision.gameObject);
        }
    }
}
