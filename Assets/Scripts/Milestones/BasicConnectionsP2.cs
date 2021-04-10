using UnityEngine;
using Power.V2;
namespace Milestones
{
    public class BasicConnectionsP2 : MilestoneBase
    {
        private GameObject[] houses;
        public override bool CheckCompleteMilestone()
        {
            Debug.Log("BasicConnections{2");
            // Find all houses
            houses = GameObject.FindGameObjectsWithTag("house");

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
            
            //Debug.Log($"Powered Houses: {poweredHouses}");
            return poweredHouses >= 5;
        }
        
        public override void SetCompleteMilestone()
        {
            base.SetCompleteMilestone();
            BuildScript bs = GameObject.FindObjectOfType<BuildScript>();
            //bs.trackApproval = true;
        }
    }
    
}