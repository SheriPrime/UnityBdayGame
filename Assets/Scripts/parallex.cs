using UnityEngine;

public class parallex : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallexEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = cam.transform.position.x * parallexEffect;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
