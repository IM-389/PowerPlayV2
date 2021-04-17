using UnityEngine;

namespace Milestones
{
    public class GettingStartedP2 : MilestoneBase
    {
        private bool hasPaused;
        private bool hasSped;
        
        public override bool CheckCompleteMilestone()
        {
            if (Time.timeScale == 0)
            {
                hasPaused = true;
            }

            if (Time.timeScale > 1)
            {
                hasSped = true;
            }

            return ((hasPaused && hasSped) && (Time.timeScale == 1));
        }
    }
}