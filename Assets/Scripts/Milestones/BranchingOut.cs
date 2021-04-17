using Power.V2;
using Milestones;
using UnityEngine;

public class BranchingOut : MilestoneBase
{
    public GameObject arrow;
    ArrowBehaviour am;
    public override bool CheckCompleteMilestone()
    {
        GameObject[] substations = GameObject.FindGameObjectsWithTag("Substation");

        if(substations.Length >= 2)
        {
            am = arrow.GetComponent<ArrowBehaviour>();
            am.FinishTheJob();
        }
        
        // Find all houses
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");

        int poweredHouses = 0;

        // Loop through all houses
        foreach (var house in houses)
        {
            // Reference to the power storage script
            ConsumerScript consumer = house.GetComponent<ConsumerScript>();
            GeneralObjectScript gos = house.GetComponent<GeneralObjectScript>();
            if (gos.isMilestoneCounted && consumer.GetManager().hasEnoughPower)
            {
                ++poweredHouses;
            }
        }
        
        return substations.Length >= 2 && poweredHouses >= 10;
    }
}
