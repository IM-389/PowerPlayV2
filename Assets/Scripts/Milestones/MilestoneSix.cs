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
        GameObject[] coalGenerators = GameObject.FindGameObjectsWithTag("coal");

        if (coalGenerators.Length >= 2)
        {
            return true;
        }

        return false;

    }
}
