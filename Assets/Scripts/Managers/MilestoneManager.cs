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

    /// <summary>
    /// Sets the data for the first milestone
    /// </summary>
    private void Start()
    {
        currentMilestones.Add(milestones[0]);
        currentMilestones[0].SetMilestoneProperties();
        milestoneText.text = currentMilestones[0].milestoneText;
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
            bool isComplete = currentMilestones[i].CompleteMilestone();
            if (isComplete)
            {
                Debug.Log("Milestone complete, setting next ones!");
                toAdd.AddRange(currentMilestones[i].nextMilestones);
                toRemove.Add(i);

                foreach (var newMilestone in currentMilestones[i].nextMilestones)
                {
                    newMilestone.SetMilestoneProperties();
                }
            }

            text += currentMilestones[i].milestoneText + "\n";
        }
        
        currentMilestones.AddRange(toAdd);
        milestoneText.text = text;
        
        foreach (var index in toRemove)
        {
            currentMilestones.RemoveAt(index);
        }
    }
}