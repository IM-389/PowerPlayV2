using UnityEngine;

namespace Milestones
{
    public class MilestoneOne : MilestoneBase
    {
        private GameObject[] houses;
        public override bool CompleteMilestone()
        {
            houses = GameObject.FindGameObjectsWithTag("house");

            int poweredHouses = 0;

            foreach (var house in houses)
            {
                if (house.GetComponent<StorageScript>().powerStored > 0)
                {
                    ++poweredHouses;
                }
            }
            
            if (poweredHouses >= 5)
            {
                return true;
            }

            return false;
        }

        public override void SetMilestoneProperties()
        {
            sequenceNumber = 1;
            milestoneName = "Basic Connections";
            milestoneText = "Connect 5 houses to a power distribution center";
        }
    }
}