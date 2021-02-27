using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    public GameObject textSet;
    public Dialogue dialogue;
    public bool tut = true;

    public bool ready = true;

    public string level = "tutorial";

    public int test;
    public void TriggerDialogue()
    {
        if (level == "Tutorial")
        {
            FindObjectOfType<DialogueBehaviour>().StartConvo(dialogue);
        }

    }
}
