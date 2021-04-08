using UnityEngine;
using Power.V2;
namespace Milestones
{
    public class OutWithTheOld : MilestoneBase
    {
        public override bool CheckCompleteMilestone()
        {
            GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

            int solarGen = 0;
        
            foreach (var generator in generators)
            {
                GeneralObjectScript gos = generator.GetComponent<GeneralObjectScript>();
                if (gos.isMilestoneCounted &&
                    generator.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_SOLAR)
                {
                    ++solarGen;
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
        
            return solarGen >= 1 && poweredHouses >= 15;
        }
    }
}