using UnityEngine;

public class platformMovement : MonoBehaviour
{
    bool isMoving = false;
    float moveSpeed = 2f;
    bool reachedTop = false;
    
    void Update()
    {
        if (isMoving && !reachedTop)
        {            // Use direction variable to control movement
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered platform trigger");
            isMoving = true;
        }
        
        if (collision.CompareTag("platformHeight"))
        {
            reachedTop = true;
        }
    }
}