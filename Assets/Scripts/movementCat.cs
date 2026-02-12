using UnityEngine;

public class movementCat : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Animator animator;

    private string lastDirection = "down";
    private bool isChopping = false;

    // Detection settings
    public float detectionRange = 1.5f; // How far to check for trees
    public LayerMask treeLayer; // Assign "Tree" layer in inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for chop input
        if (Input.GetKey(KeyCode.F) && !isChopping)
        {
            TryChop();
            return;
        }

        if (!isChopping)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * 2f;
        verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * 2f;

        animator.SetBool("right", false);
        animator.SetBool("left", false);
        animator.SetBool("up", false);
        animator.SetBool("down", false);

        if (horizontalInput > 0)
        {
            animator.SetBool("right", true);
            lastDirection = "right";
        }
        else if (horizontalInput < 0)
        {
            animator.SetBool("left", true);
            lastDirection = "left";
        }
        else if (verticalInput > 0)
        {
            animator.SetBool("up", true);
            lastDirection = "up";
        }
        else if (verticalInput < 0)
        {
            animator.SetBool("down", true);
            lastDirection = "down";
        }

        transform.Translate(horizontalInput, verticalInput, 0);
    }
    
    void TryChop()
    {
        // Check all 4 directions for trees
        Vector2[] directions = {
            Vector2.right,  // Right
            Vector2.left,   // Left
            Vector2.up,     // Up
            Vector2.down    // Down
        };
        
        string[] directionNames = { "right", "left", "up", "down" };
        
        // Find the closest tree in any direction
        GameObject closestTree = null;
        float closestDistance = detectionRange;
        string treeDirection = "";
        
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], detectionRange, treeLayer);
            
            if (hit.collider != null)
            {
                float distance = hit.distance;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTree = hit.collider.gameObject;
                    treeDirection = directionNames[i];
                }
            }
        }
        
        // If tree found, chop in that direction
        if (closestTree != null)
        {
            Debug.Log("Tree found " + treeDirection + " - Chopping!");
            StartChop(treeDirection);
        }
        else
        {
            Debug.Log("No tree nearby!");
        }
    }
    
    void StartChop(string direction)
    {
        isChopping = true;
        
        // Reset all movement bools
        animator.SetBool("right", false);
        animator.SetBool("left", false);
        animator.SetBool("up", false);
        animator.SetBool("down", false);
        
        // Trigger chop animation in detected direction
        if (direction == "right")
        {
            animator.SetTrigger("chopRight");
        }
        else if (direction == "left")
        {
            animator.SetTrigger("chopLeft");
        }
        else if (direction == "up")
        {
            animator.SetTrigger("chopUp");
        }
        else if (direction == "down")
        {
            animator.SetTrigger("chopDown");
        }
        
        Invoke("EndChop", 0.5f);
    }
    
    void EndChop()
    {
        isChopping = false;
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        // Draw lines showing detection range
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionRange);
    }
}