using System.Collections.Generic;
using UnityEngine;

public class DialogueBehaviour : MonoBehaviour
{
    List<string> sentences;

    //public TutorialBehaviour tb;
    public StoryTelling st;
    public GameManager gm;
    public SceneController sc;

    void Start()
    {
        sentences = new List<string>();
        gm = GameObject.FindObjectOfType<GameManager>();
        sc = GameObject.FindObjectOfType<SceneController>();
    }

    public void StartConvo(Dialogue dialog)
    {
        st = GameObject.FindObjectOfType<StoryTelling>();

        sentences = new List<string>();
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Add(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            //gc.tutorialReady = false;
            //EndDialogue();
            return;
        }
        string talk = sentences[0];
        sc.dialogueLine.text = talk;
        sentences.RemoveAt(0);
    }

}

    /*void EndDialogue()
    {
        if (st.tut)
        {
            Destroy(st.textSet);
            //tb.tutorialStarted = false;
            //tb.goToNext = false;
            //tb.dialoguePanel.SetActive(false);
            //tb.LetPlayerMove(true);
            Destroy(st.gameObject);
        }
    }*/


