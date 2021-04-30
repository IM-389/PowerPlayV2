using Power.V2;
using Milestones;
using UnityEngine;

public class BranchingOutP1 : MilestoneBase
{
    public GameObject arrow;
    ArrowBehaviour am;
    public override bool CheckCompleteMilestone()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
        
        return generators.Length >= 2;
    }
}
