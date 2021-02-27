using System.Collections.Generic;
using UnityEngine;

public class DialogueBehaviour : MonoBehaviour
{
    List<string> sentences;

    //public TutorialBehaviour tb;
    public StoryTelling st;
    public GameController gc;

    void Start()
    {
        sentences = new List<string>();
        //tb = GameObject.FindObjectOfType<TutorialBehaviour>();
        gc = GameObject.FindObjectOfType<GameController>();
    }

    public void StartConvo(Dialogue dialog)
    {
        st = GameObject.FindObjectOfType<StoryTelling>();
        //Debug.Log("Starting convo");

        sentences.Clear();

        //print(sentences.Count);
        foreach (string sentence in dialog.sentences)
        {
            sentences.Add(sentence);
        }
        //print("test = " + st.test);
        //print(sentences.Count);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            gc.tutorialReady = false;
            EndDialogue();
            return;
        }
        if (st.tut)
        {
            string talk = sentences[0];
            tb.dialogueLine.text = talk;
            sentences.RemoveAt(0);
        }

    }

    void EndDialogue()
    {
        if (st.tut)
        {
            Destroy(st.textSet);
            //st.textSet.SetActive(false);
            tb.tutorialStarted = false;
            tb.goToNext = false;
            tb.dialoguePanel.SetActive(false);
            //Debug.Log("end convo");
            tb.LetPlayerMove(true);
            Destroy(st.gameObject);
        }

    }
}
