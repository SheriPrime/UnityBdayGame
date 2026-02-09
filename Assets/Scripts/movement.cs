using UnityEngine;

public class movement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    private Animator animator;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * 2f;
        verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * 2f;

        animator.SetBool("right", horizontalInput > 0);
        animator.SetBool("left", horizontalInput < 0);
        animator.SetBool("up", verticalInput > 0);
        animator.SetBool("down", verticalInput < 0);

        rb.MovePosition(rb.position + new Vector2(horizontalInput, verticalInput));
    }
}