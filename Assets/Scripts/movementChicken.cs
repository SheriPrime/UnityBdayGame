using UnityEngine;

public class movementChicken : MonoBehaviour
{
    public float flapForce = 5f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {

        transform.Translate(Vector2.right * Time.deltaTime * flapForce);
        // Flap on space press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * flapForce;
        }
        
        // Rotate based on velocity
        float angle = Mathf.Clamp(rb.velocity.y * 3f, -90f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Game Over
        Debug.Log("Game Over!");
        Time.timeScale = 0; // Pause game
    }
}