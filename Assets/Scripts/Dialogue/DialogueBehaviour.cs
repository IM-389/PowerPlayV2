using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehaviour : MonoBehaviour
{
    private List<string> sentences = new List<string>();

    //public TutorialBehaviour tb;
    public string nextSetMessage;
    public StoryTelling st;
    public GameManager gm;
    public SceneController sc;
    public GameObject mainButton;
    public GameObject nextSetButton;

    public GameObject dialougePanel;
    public GameObject griddy;
    public Animator griddyAnim;
    public BoxCollider2D pauseBlocker;

    public GameObject arrow;
    
    // Reference to location of sound
    [FMODUnity.EventRef]
    public string griddyAffirmative;

    void Start()
    {
        //sentences = new List<string>();
        gm = GameObject.FindObjectOfType<GameManager>();
        sc = GameObject.FindObjectOfType<SceneController>();
        st = GameObject.FindObjectOfType<StoryTelling>();
        // Ugly method of getting the correct animator
        //griddyAnim = griddy.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    public void StartConvo(Dialogue dialog)
    {
        dialougePanel.SetActive(true);
        st = GameObject.FindObjectOfType<StoryTelling>();

        FMODUnity.RuntimeManager.PlayOneShot(griddyAffirmative);

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

        FaceScript.faceSprite = 2;
        FaceScript.switchFaces = true;

        //Debug.Log("Calling DisplayNextSentence!");
        //Debug.Log($"Number items in list: {sentences.Count}");
        if (sentences.Count == 0)
        {
            //sc.dialogueLine.text = nextSetMessage;
            //nextSetButton.SetActive(true);
            //mainButton.SetActive(false);
            FaceScript.faceSprite = 1;
            FaceScript.switchFaces = true;
            dialougePanel.SetActive(false);
            griddyAnim.SetBool("leaving", true);
            pauseBlocker.enabled = false;
            StartCoroutine(HideGriddy());
            Time.timeScale = 1;
            return;

        }
        griddyAnim.SetTrigger("talkingTrigger");
        string talk = sentences[0];
        sc.dialogueLine.text = talk;
        Time.timeScale = 0;
        sentences.RemoveAt(0);
    }

    public void EndDialogue()
    {
        if(st.nextTextSet == null)
        {
            sc.dialogueLine.text = "All done! Good job!";
            nextSetButton.SetActive(false);
            mainButton.SetActive(false);
            pauseBlocker.enabled = true;
            Time.timeScale = 1;
            return;
        }
        print("hi?");
        st.nextTextSet.SetActive(true);
        Destroy(st.textSet);
        st = GameObject.FindObjectOfType<StoryTelling>();
        st.TriggerDialogue();
        
    }

    private IEnumerator HideGriddy()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        griddy.SetActive(false);
        arrow.SetActive(true);
    }
}
    



