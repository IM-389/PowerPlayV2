using UnityEngine;

namespace Milestones
{
    public class MilestoneOneHalf : MilestoneBase
    {
        public override bool CheckCompleteMilestone()
        {
            GameObject[] powerPoles = GameObject.FindGameObjectsWithTag("Power");

            return powerPoles.Length > 0;
        }
    }
}