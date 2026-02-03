using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;          // Reference to the Animator (Crossfade or other)
    public float transitionTime = 1f;    // Seconds the fade animation lasts
    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadLevel(nextScene));
    }

    private System.Collections.IEnumerator LoadLevel(int levelIndex)
    {
        // Play the start‑fade animation
        transition.SetTrigger("Start");

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the target scene
        SceneManager.LoadScene(levelIndex);
    }
}
