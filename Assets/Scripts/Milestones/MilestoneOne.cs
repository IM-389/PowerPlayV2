using UnityEngine;

namespace Milestones
{
    public class MilestoneOne : MilestoneBase
    {
        private GameObject[] houses;
        public override bool CheckCompleteMilestone()
        {
            // Find all houses
            houses = GameObject.FindGameObjectsWithTag("house");

            int poweredHouses = 0;

            // Loop through all houses
            foreach (var house in houses)
            {
                // Reference to the power storage script
                StorageScript storage = house.GetComponent<StorageScript>();
                if (storage.isMilestoneCounted && storage.powerStored > 0)
                {
                    ++poweredHouses;
                }
            }
            
            return poweredHouses >= 5;
        }

        public override void SetMilestoneProperties()
        {
            sequenceNumber = 1;
            milestoneName = "Basic Connections";
            milestoneText = "Connect 5 houses to a power distribution center";
        }
    }
}