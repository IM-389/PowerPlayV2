using UnityEngine;
using Power.V2;
namespace Milestones
{
    public class InWithTheNew : MilestoneBase
    {
        public override bool CheckCompleteMilestone()
        {
            GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

            int windGen = 0;
        
            foreach (var generator in generators)
            {
                GeneralObjectScript gos = generator.GetComponent<GeneralObjectScript>();
                if (gos.isMilestoneCounted &&
                    generator.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_WIND)
                {
                    ++windGen;
                }
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
        
            return windGen >= 1 && poweredHouses >= 22;
        }
    }
}