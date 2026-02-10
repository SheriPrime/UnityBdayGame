using UnityEngine;

public class menuController : MonoBehaviour
{
    public GameObject menuUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuUI.SetActive(!menuUI.activeSelf);
        }
    }
}
