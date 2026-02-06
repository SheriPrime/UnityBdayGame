using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    public string sceneToLoad;
    public LevelLoader levelLoader;
    private bool isLoading = false;

    public void Interact()
    {
        if (!isLoading && !string.IsNullOrEmpty(sceneToLoad) && levelLoader != null)
        {
            isLoading = true;
            StartCoroutine(LoadWithTransition());
        }
    }

    public string GetInteractPrompt()
    {
        return "Press E to enter";
    }

    private IEnumerator LoadWithTransition()
    {
        levelLoader.transition.SetTrigger("Start");
        yield return new WaitForSeconds(levelLoader.transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}