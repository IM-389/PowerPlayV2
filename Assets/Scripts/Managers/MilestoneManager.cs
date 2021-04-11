using System.Collections.Generic;
using Milestones;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneManager : MonoBehaviour
{

    [Tooltip("Collection of all the milestones, ordered from first to last")]
    public MilestoneBase[] milestones;

    /// <summary>
    /// Which milestone the player is currently on
    /// </summary>
    public MilestoneBase currentMilestones;

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
        currentMilestones = milestones[0];
        milestoneText.text = currentMilestones.milestoneText;
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
        bool isComplete = currentMilestones.CheckCompleteMilestone();
        if (isComplete || Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Milestone complete, setting next ones!");
            currentMilestones.SetCompleteMilestone();
            
            foreach (var building in currentMilestones.newBuildings)
            {
                build.spawnableBuildings.Add(building);
            }
            //build.SetupDropdown();
            currentMilestones = currentMilestones.nextMilestones[0];
            
            griddy.SetActive(true);
            if (currentMilestones.hasQuestion)
            {
                quizManager.StartQuiz();
            }

            dialouge = dialouge.nextTextSet.GetComponent<StoryTelling>();
            dialouge.TriggerDialogue();
        }
        if(currentMilestones.startDay > -1)
        {
           currentMilestones.daysElapsed = timeManager.days - currentMilestones.startDay;
        }
        text += currentMilestones.milestoneText + "\n";
        
        milestoneText.text = text;
    }
}
