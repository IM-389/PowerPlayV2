using Power.V2;
using Milestones;
using UnityEngine;

public class PoweringUp : MilestoneBase
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
        
        // Find all houses
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");

        int poweredHouses = 0;

        if (coalGen >= 1)
        {
            am = arrow.GetComponent<ArrowBehaviour>();
            am.FinishTheJob();
        }

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
        
        return coalGen >= 1 && poweredHouses >= 14;
    }
}
