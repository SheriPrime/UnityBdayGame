using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    public GameObject eKeyPrompt;
    public string sceneToLoad;
    public LevelLoader levelLoader;
    
    private bool playerInRange = false;
    private bool isLoading = false;

    void Start()
    {
        // Make sure the prompt is hidden at start
        if (eKeyPrompt != null)
            eKeyPrompt.SetActive(false);
    }

    void Update()
    {
        // Check if player is in range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isLoading)
        {
            LoadNextScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(false);
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad) && levelLoader != null)
        {
            isLoading = true;
            StartCoroutine(LoadWithTransition());
        }
    }

    private System.Collections.IEnumerator LoadWithTransition()
    {
        levelLoader.transition.SetTrigger("Start");
        yield return new WaitForSeconds(levelLoader.transitionTime);
        
        SceneManager.LoadScene(sceneToLoad);
    }
}