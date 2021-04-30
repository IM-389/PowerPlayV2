using Power.V2;
using Milestones;
using UnityEngine;

public class BranchingOutP2 : MilestoneBase
{
    public GameObject arrow;
    ArrowBehaviour am;
    public override bool CheckCompleteMilestone()
    {
        GameObject[] substations = GameObject.FindGameObjectsWithTag("Substation");
        /*
        if(substations.Length >= 2)
        {
            am = arrow.GetComponent<ArrowBehaviour>();
            am.FinishTheJob();
        }
        */
        return substations.Length >= 2;
    }
}
