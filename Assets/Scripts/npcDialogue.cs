using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue/NPC Dialogue")]
public class npcDialogue : ScriptableObject
{
    public enum Speaker { NPC, Player }
    
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public Speaker[] speakers; // Who is speaking each line
    public bool[] autoAdvance;
    public float typingSpeed = 0.05f;
    public AudioClip typingSound;
    public float voicePitch = 1.0f;
    public float autoAdvanceDelay = 2.0f;
}