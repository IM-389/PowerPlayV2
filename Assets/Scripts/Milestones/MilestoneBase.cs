using System.Collections.Generic;
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

        [Tooltip("What milestone(s) are immediatly after this one")]
        public List<MilestoneBase> nextMilestones = new List<MilestoneBase>();
        
        public abstract bool CompleteMilestone();

        public abstract void SetMilestoneProperties();
    }
}