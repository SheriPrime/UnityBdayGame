using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC Dialogue")]
public class npcDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoAdvance; // Whether each line should auto-advance after a delay
    public float typingSpeed = 0.05f; // Time between each character appearing
    public AudioClip typingSound; // Sound to play while typing
    public float voicePitch = 1.0f; // Pitch of the NPC's voice
    public float autoAdvanceDelay = 2.0f; // Time to wait before auto-advancing to the next line
    
}
