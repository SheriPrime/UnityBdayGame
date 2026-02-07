using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject eKeyPrompt; // This is now a child of the player
    private IInteractable currentInteractable;
    private bool canInteract = false;

    void Start()
    {
        if (eKeyPrompt != null)
            eKeyPrompt.SetActive(false);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable?.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        
        if (interactable != null)
        {
            currentInteractable = interactable;
            canInteract = true;
            
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            canInteract = false;
            
            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(false);
        }
    }
}