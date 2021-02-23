using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class MilestoneSix : MilestoneBase
{
    public override void SetMilestoneProperties()
    {
        sequenceNumber = 6;
        milestoneName = "Enter the Smart Grid";
        milestoneText = "Place and connect a second coal generator";
    }

    public override bool CompleteMilestone()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

        int numCoal = 0;

        foreach (var generator in generators)
        {
            if (generator.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_COAL)
            {
                ++numCoal;
            }
        }

        return (numCoal >= 2);

    }
}
