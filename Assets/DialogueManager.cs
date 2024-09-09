using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    
    public MonoBehaviour scriptToDisable;
    public GameObject[] hiddenWalls;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private void Awake()
    {
       
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = false;
        }
        isDialogueActive = true;

        animator.Play("PopUpText");

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        Debug.Log("Lines remaining: " + lines.Count);
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();
        Debug.Log("Displaying line: " + currentLine.line);

        characterName.text = currentLine.character.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }


    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = true;
        }
        isDialogueActive = false;
        animator.Play("Hide");
        EventSystem.current.SetSelectedGameObject(null);

        foreach (GameObject obj in hiddenWalls)
        {
            if (obj.TryGetComponent<BoxCollider2D>(out var objCollider))
            {
                objCollider.enabled = false;
            }
        }
        
    }
}