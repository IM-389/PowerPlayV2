
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueLine;

    public bool goToNext = true;
    public bool canPress = true;
    public bool canPlace;

    public string[] sentences;

    public float waitTime = 1;

    public int currentSet = 1;

    public StoryTelling st;
    public DialogueBehaviour db;

    [FMODUnity.EventRef]
    public string griddySounds;
    
    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.FindObjectOfType<DialogueBehaviour>();
        st = GameObject.FindObjectOfType<StoryTelling>();
        st.TriggerDialogue();

        canPress = false;
        Invoke("WaitToPress", waitTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressButtonForText()
    {
            db.DisplayNextSentence();
            FMODUnity.RuntimeManager.PlayOneShot(griddySounds);
            canPress = false;
            Invoke("WaitToPress", waitTime);


    }

    public void ContinueTutorial()
    {
        goToNext = true;
    }

    void WaitToPress()
    {
        canPress = true;
    }
}
