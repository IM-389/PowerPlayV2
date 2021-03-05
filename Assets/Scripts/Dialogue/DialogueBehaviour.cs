using System.Collections.Generic;
using UnityEngine;

public class DialogueBehaviour : MonoBehaviour
{
    List<string> sentences;

    //public TutorialBehaviour tb;
    public string nextSetMessage;
    public StoryTelling st;
    public GameManager gm;
    public SceneController sc;
    public GameObject mainButton;
    public GameObject nextSetButton;

    public GameObject dialougePanel;
    void Start()
    {
        sentences = new List<string>();
        gm = GameObject.FindObjectOfType<GameManager>();
        sc = GameObject.FindObjectOfType<SceneController>();
        st = GameObject.FindObjectOfType<StoryTelling>();
    }

    public void StartConvo(Dialogue dialog)
    {
        dialougePanel.SetActive(true);
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
            //sc.dialogueLine.text = nextSetMessage;
            //nextSetButton.SetActive(true);
            //mainButton.SetActive(false);
            dialougePanel.SetActive(false);
            return;
        }
        string talk = sentences[0];
        sc.dialogueLine.text = talk;
        sentences.RemoveAt(0);
    }

    public void EndDialogue()
    {
        if(st.nextTextSet == null)
        {
            sc.dialogueLine.text = "All done! Good job!";
            nextSetButton.SetActive(false);
            mainButton.SetActive(false);
            return;
        }
        print("hi?");
        st.nextTextSet.SetActive(true);
        Destroy(st.textSet);
        st = GameObject.FindObjectOfType<StoryTelling>();
        st.TriggerDialogue();
        
    }
}
    



