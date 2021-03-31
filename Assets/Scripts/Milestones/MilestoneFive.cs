using Power.V2;
using Milestones;
using UnityEngine;

public class MilestoneFive : MilestoneBase
{

    public override bool CheckCompleteMilestone()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

        int numCoal = 0;

        foreach (var generator in generators)
        {
            if (generator.GetComponent<GeneralObjectScript>().isMilestoneCounted &&
                generator.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_COAL)
            {
                ++numCoal;
            }
        }

        return (numCoal >= 2);

    }
}
