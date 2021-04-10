using Power.V2;
using Milestones;
using UnityEngine;

public class BranchingOut : MilestoneBase
{
    public override bool CheckCompleteMilestone()
    {
        GameObject[] substations = GameObject.FindGameObjectsWithTag("Substation");
        
        // Find all houses
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");

        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
        
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
        
        return substations.Length >= 2 && poweredHouses >= 10 && generators.Length >= 2;
    }
}
