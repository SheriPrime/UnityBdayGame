using UnityEngine;

public class movement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * 5f;

        transform.Translate(horizontalInput, verticalInput, 0);
    }
}
