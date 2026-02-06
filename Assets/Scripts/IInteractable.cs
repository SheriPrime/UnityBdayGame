using UnityEngine;

public interface IInteractable
{
    void Interact();
    string GetInteractPrompt(); // Optional: for different prompts like "Press E to open", "Press E to talk", etc.
}