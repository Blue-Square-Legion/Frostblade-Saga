using System.Collections.Generic;
using UnityEngine;



[System.Serializable]

public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}



public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;  // The dialogue for this trigger
    public DialogueManager dialogueManager;  // Reference to the specific manager handling this dialogue

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);  // Start dialogue using assigned manager
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
            GetComponent<Collider2D>().enabled = false;
        }
    }


}