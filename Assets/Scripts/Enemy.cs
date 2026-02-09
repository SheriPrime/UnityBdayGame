using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    private float direction = 1f;

    [Header("References")]
    public GameObject topCollider; // Reference to the child collider

    void Update()
    {
        // Patrol movement
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hit patrol boundaries
        if (other.CompareTag("rightCollider") || other.CompareTag("leftCollider"))
        {
            direction *= -1f;
            
            // Flip sprite
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}