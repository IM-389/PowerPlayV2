using UnityEngine;

namespace Milestones
{
    public class GettingStarted : MilestoneBase
    {
        public override bool CheckCompleteMilestone()
        {
            GameObject[] powerPoles = GameObject.FindGameObjectsWithTag("Power");

            return powerPoles.Length == 0;
        }
    }
}