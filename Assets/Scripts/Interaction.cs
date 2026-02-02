using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    public GameObject eKeyPrompt;
    public string sceneToLoad;
    public float fadeDuration = 1f;
    
    private bool playerInRange = false;
    private bool isLoading = false;

    void Start()
    {
        if (eKeyPrompt != null)
            eKeyPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isLoading)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(false);
        }
    }
}