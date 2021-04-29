using Power.V2;
using Milestones;
using UnityEngine;

public class PoweringUpP2 : MilestoneBase
{
    public GameObject arrow;
    ArrowBehaviour am;
    public override bool CheckCompleteMilestone()
    {
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
        
        return poweredHouses >= 15;
    }
}
