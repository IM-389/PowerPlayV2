using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    public GameObject textSet;
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueBehaviour>().StartConvo(dialogue);
    }
}
