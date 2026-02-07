using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public npcDialogue dialogue;
    public GameObject dialogueUI;
    public TMP_Text dialogueText, nameText;
    public Image npcPortrait;

    public string sceneToLoad;
    public LevelLoader levelLoader;
    private bool isLoading = false;

    private int currentLineIndex = 0;
    private bool isDialogueActive, isTyping;

    public string GetInteractPrompt()
    {
        return "Press E to talk";
    }

    public void Interact()
    {
        if (dialogue == null && !isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadWithTransition());
            return;
        }

        if (isDialogueActive)
        {
            ContinueDialogue();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentLineIndex = 0;
        nameText.text = dialogue.npcName;
        npcPortrait.sprite = dialogue.npcPortrait;
        dialogueUI.SetActive(true);

        StartCoroutine(ShowCurrentDialogueLine());
    }

    void ContinueDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogue.dialogueLines[currentLineIndex]);
            isTyping = false;
        }
        else
        {
            if (++currentLineIndex < dialogue.dialogueLines.Length)
            {
                StartCoroutine(ShowCurrentDialogueLine());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    IEnumerator ShowCurrentDialogueLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogue.dialogueLines[currentLineIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogue.typingSpeed);
        }

        isTyping = false;

        if (dialogue.autoAdvance.Length > currentLineIndex && dialogue.autoAdvance[currentLineIndex])
        {
            yield return new WaitForSeconds(dialogue.autoAdvanceDelay);
            ContinueDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueText.SetText("");
        dialogueUI.SetActive(false);
        
        // Load next scene if one is specified
        if (!string.IsNullOrEmpty(sceneToLoad) && levelLoader != null && currentLineIndex >= dialogue.dialogueLines.Length)
        {
            StartCoroutine(LoadWithTransition());
        }
    }

    private IEnumerator LoadWithTransition()
    {
        levelLoader.transition.SetTrigger("Start");
        yield return new WaitForSeconds(levelLoader.transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
