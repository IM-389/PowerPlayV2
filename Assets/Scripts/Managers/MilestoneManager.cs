using System.Collections.Generic;
using Milestones;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneManager : MonoBehaviour
{

    [Tooltip("Collection of all the milestones, ordered from first to last")]
    public MilestoneBase[] milestones;

    /// <summary>
    /// Which milestones the player are currently on
    /// </summary>
    public List<MilestoneBase> currentMilestones = new List<MilestoneBase>();

    [Tooltip("Text displaying the current milestone to the player")]
    public Text milestoneText;

    public Text timeText;

    public BuildScript build;

    public StoryTelling dialouge;

    [Tooltip("Object for Griddy")]
    public GameObject griddy;

    [Tooltip("Reference to the QuizManager")]
    public QuizManager quizManager;

    public TimeManager timeManager;
    
    /// <summary>
    /// Sets the data for the first milestone
    /// </summary>
    private void Start()
    {
        currentMilestones.Add(milestones[0]);
        milestoneText.text = currentMilestones[0].milestoneText;
        build = GameObject.FindWithTag("Background").GetComponent<BuildScript>();
    }
    
    /// <summary>
    /// Checks if each milestone is complete
    /// </summary>
    void Update()
    {
        List<MilestoneBase> toAdd = new List<MilestoneBase>();
        List<int> toRemove = new List<int>();
        string text = "";
        for(int i = 0; i < currentMilestones.Count; ++i)
        {
            bool isComplete = currentMilestones[i].CheckCompleteMilestone();
            if (isComplete || Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("Milestone complete, setting next ones!");
                
                currentMilestones[i].SetCompleteMilestone();
                
                foreach (var building in currentMilestones[i].newBuildings)
                {
                    build.spawnableBuildings.Add(building);
                }
                //build.SetupDropdown();
                toAdd.AddRange(currentMilestones[i].nextMilestones);
                toRemove.Add(i);
                
                griddy.SetActive(true);
                if (currentMilestones[i].hasQuestion)
                {
                    quizManager.StartQuiz();
                }

                dialouge = dialouge.nextTextSet.GetComponent<StoryTelling>();
                dialouge.TriggerDialogue();
            }
            if(currentMilestones[i].startDay > -1)
            {
               currentMilestones[i].daysElapsed = timeManager.days - currentMilestones[i].startDay;
            }
            text += currentMilestones[i].milestoneText + "\n";
        }
        
        currentMilestones.AddRange(toAdd);
        milestoneText.text = text;
        
        foreach (var index in toRemove)
        {
            Debug.Log("in foreach at top-see this, borked");
            currentMilestones.RemoveAt(index);
            Debug.Log("in foreach at bottom-see this, borked");
        }
    }
}
