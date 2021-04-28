using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Power.V2;
using Milestones;

public class GoingGreen : MilestoneBase
{
    private bool generatorRemoved = false;
    public override bool CheckCompleteMilestone()
    {
        GeneratorScript[] generators = GameObject.FindObjectsOfType<GeneratorScript>();
        if (!generatorRemoved)
        {

            foreach (var generator in generators)
            {
                if (!generator.GetComponent<GeneralObjectScript>().isMilestoneCounted)
                {
                    return false;
                }
            }
        }

        generatorRemoved = true;
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");
        
        int poweredHouses = 0;
        foreach (var house in houses)
        {
            if (house.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
            {
                ++poweredHouses;
            }
        }

        foreach (var generator in generators)
        {
            if (generator.type == PowerManager.POWER_TYPES.TYPE_COAL)
            {
                return false;
            }
        }
        
        return poweredHouses >= 45;
    }
}