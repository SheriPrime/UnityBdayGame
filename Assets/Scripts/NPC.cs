using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public npcDialogue dialogue;
    public GameObject dialogueUI;
    public TMP_Text dialogueText, nameText;
    public Image npcPortrait;

    private int currentLineIndex = 0;
    private bool isDialogueActive, isTyping;

    public string GetInteractPrompt()
    {
        return !isDialogueActive ? "Talk" : "Continue";
    }

    public void Interact()
    {
        if (!isDialogueActive || dialogue == null)
        {
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
    }
}
