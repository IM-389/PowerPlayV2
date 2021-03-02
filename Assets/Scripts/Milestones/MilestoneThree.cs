using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class MilestoneThree : MilestoneBase
{
    public override bool CheckCompleteMilestone()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

        int coalGen = 0;
        
        foreach (var generator in generators)
        {
            if (generator.GetComponent<StorageScript>().isMilestoneCounted &&
                generator.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_COAL)
            {
                ++coalGen;
            }
        }
        
        // Find all houses
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");

        int poweredHouses = 0;

        // Loop through all houses
        foreach (var house in houses)
        {
            // Reference to the power storage script
            StorageScript storage = house.GetComponent<StorageScript>();
            if (storage.isMilestoneCounted && storage.powerStored > 0)
            {
                ++poweredHouses;
            }
        }
        
        return coalGen >= 1 && poweredHouses >= 10;
    }

    public override void SetMilestoneProperties()
    {
        sequenceNumber = 1;
        milestoneName = "Branching Out";
        milestoneText = "Create a new substation, and use it to power the new houses";
    }
}
