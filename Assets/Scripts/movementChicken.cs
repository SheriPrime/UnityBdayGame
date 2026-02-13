using UnityEngine;
using UnityEngine.SceneManagement;

public class movementChicken : MonoBehaviour
{
    public float flapForce = 5f;
    private Rigidbody2D rb;

    public GameObject camMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        camMove.transform.Translate(Vector2.right * Time.deltaTime * flapForce);
        transform.Translate(Vector2.right * Time.deltaTime * flapForce);
        // Flap on space press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = Vector2.up * flapForce;
        }
        
        // Rotate based on velocity
        float angle = Mathf.Clamp(rb.linearVelocity.y * 3f, -90f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        // Game Over
        Debug.Log("Game Over!");
        Time.timeScale = 0; // Pause game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Resume game
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Finish"))
        {
            Debug.Log("Reached Finish Line!");
        }
    }
}