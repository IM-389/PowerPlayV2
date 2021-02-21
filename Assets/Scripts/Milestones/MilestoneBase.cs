using UnityEngine;

namespace Milestones
{
    public abstract class MilestoneBase : MonoBehaviour
    {
        /// <summary>
        /// Order the milestones come in
        /// </summary>
        public int sequenceNumber;

        /// <summary>
        /// What text shows on the milestone
        /// </summary>
        public string milestoneText;

        public string milestoneName;
        
        public abstract bool CompleteMilestone();

        public abstract void SetMilestoneProperties();
    }
}