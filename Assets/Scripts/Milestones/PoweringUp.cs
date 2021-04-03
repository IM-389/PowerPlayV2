using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class PoweringUp : MilestoneBase
{
    public override bool CheckCompleteMilestone()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

        int coalGen = 0;
        
        foreach (var generator in generators)
        {
            GeneralObjectScript gos = generator.GetComponent<GeneralObjectScript>();
            if (gos.isMilestoneCounted &&
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
            GeneralObjectScript gos = house.GetComponent<GeneralObjectScript>();
            if (gos.isMilestoneCounted && storage.powerStored > 0)
            {
                ++poweredHouses;
            }
        }
        
        return coalGen >= 1 && poweredHouses >= 15;
    }
}
