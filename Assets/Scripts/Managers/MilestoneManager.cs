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
    private int currentMilestone = 0;

    [Tooltip("Text displaying the current milestone to the player")]
    public Text milestoneText;

    /// <summary>
    /// Sets the data for the first milestone
    /// </summary>
    private void Start()
    {
        milestones[currentMilestone].SetMilestoneProperties();
        milestoneText.text = milestones[currentMilestone].milestoneText;
    }
    
    /// <summary>
    /// Checks if each milestone is complete
    /// </summary>
    void Update()
    {
        bool isComplete = milestones[currentMilestone].CompleteMilestone();
        if (isComplete)
        {
            Debug.Log("Next milestone!");
            ++currentMilestone;
            milestones[currentMilestone].SetMilestoneProperties();
            milestoneText.text = milestones[currentMilestone].milestoneText;
        }
    }
}
