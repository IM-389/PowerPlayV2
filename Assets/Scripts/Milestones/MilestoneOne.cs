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
                GeneralObjectScript gos = house.GetComponent<GeneralObjectScript>();
                if (gos.isMilestoneCounted && storage.powerStored > 0)
                {
                    ++poweredHouses;
                }
            }
            
            //Debug.Log($"Powered Houses: {poweredHouses}");
            return poweredHouses >= 5;
        }
        
        public override void SetCompleteMilestone()
        {
            base.SetCompleteMilestone();
            BuildScript bs = GameObject.FindObjectOfType<BuildScript>();
            bs.trackApproval = true;
        }
    }
    
}