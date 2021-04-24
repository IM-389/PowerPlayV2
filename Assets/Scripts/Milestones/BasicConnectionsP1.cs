using UnityEngine;

namespace Milestones
{
    public class BasicConnectionsP1 : MilestoneBase
    {
        public override bool CheckCompleteMilestone()
        {
            //Debug.Log("BasicConnectionsP1");
            GameObject[] powerPoles = GameObject.FindGameObjectsWithTag("Power");

            return powerPoles.Length >= 5;
        }
    }
}