using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    public GameObject textSet;
    public GameObject nextTextSet;
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        Debug.Log($"Called from {gameObject.name}");
        FindObjectOfType<DialogueBehaviour>().StartConvo(dialogue);
    }
}
