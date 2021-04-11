using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    public GameObject textSet;
    public GameObject nextTextSet;
    public Dialogue dialogue;
    private BoxCollider2D pauseBlocker;
    public void TriggerDialogue(bool hasQuiz)
    {
        //Debug.Log($"Called from {gameObject.name}");
        pauseBlocker = GameObject.FindWithTag("PauseBlocker").GetComponent<BoxCollider2D>();
        pauseBlocker.enabled = true;
        FindObjectOfType<DialogueBehaviour>().StartConvo(dialogue, hasQuiz);
    }
}
