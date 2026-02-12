using UnityEngine;

public class parallex : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallexEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float movement = cam.transform.position.x * (1 - parallexEffect);
        float dist = cam.transform.position.x * parallexEffect;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    
        if(movement > startPos + length) startPos += length;
        else if(movement < startPos - length) startPos -= length;
    }
}
