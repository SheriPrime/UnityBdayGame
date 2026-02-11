using UnityEngine;

public class movementBunny : MonoBehaviour
{
    private float horizontalInput;
    public float jumpForce;
    public float moveSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isGrounded = true;

    // Reference to the bounce force
    public float bounceForce = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        transform.Translate(horizontalInput, 0, 0);
        if (horizontalInput !=0)
        {
            // Debug.Log("H input != 0");
            animator.SetBool("isRunning", true);
            if (horizontalInput > 0)
            {
                // Debug.Log("H input > 0");
                transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); // Face right
            }
            else if (horizontalInput < 0)
            {
                // Debug.Log("H input < 0");
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Face left
            }
        }
        else
        {
            // Debug.Log("H input = 0");
            animator.SetBool("isRunning", false);
        }

        if (Input.GetAxis("Jump") != 0 && isGrounded)
        {
            animator.SetBool("isJumping", true);
            Jump();
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
            // Perform action for Space key
            animator.SetBool("chop_right", true);
        }
        else
        {
            animator.SetBool("chop_right", false);
        }
    }

    void Jump()
    {
        // Apply an upward force as an impulse for an immediate jump
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false; // Player is now in the air
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && rb.linearVelocity.y < 0)
        {
            // Get the Enemy script from the parent
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            }
        }
        if (other.CompareTag("coin"))
        {
            CoinUI.coinCount++;
            Destroy(other.gameObject);
        }
    }
}
