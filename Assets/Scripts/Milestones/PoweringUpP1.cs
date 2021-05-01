using Power.V2;
using Milestones;
using UnityEngine;

public class PoweringUpP1 : MilestoneBase
{
    public GameObject arrow;
    ArrowBehaviour am;
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
        
        return coalGen >= 2;
    }
}
