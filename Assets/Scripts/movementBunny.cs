using UnityEngine;

public class movementBunny : MonoBehaviour
{
    private float horizontalInput;
    private float jumpForce = 4f;
    private float moveSpeed = 4f;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        transform.Translate(horizontalInput, 0, 0);
        if (horizontalInput > 0)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); // Face right
        }
        else if (horizontalInput < 0)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Face left
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * jumpForce * Time.deltaTime;
            animator.SetBool("isJumping", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isJumping", false);
        }
    }
}
