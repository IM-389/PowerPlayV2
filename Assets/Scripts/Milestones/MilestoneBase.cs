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

        [Tooltip("What areas/parts the map get unlocked when the milestone is completed")]
        public GameObject[] unlockedObjects;

        [Tooltip("Which sections of fog will be removed upon milestone completion")]
        public GameObject[] removedFog;

        [Tooltip("What new buildings to add")]
        public GameObject[] newBuildings;
        
        [Tooltip("What milestone(s) are immediatly after this one")]
        public List<MilestoneBase> nextMilestones = new List<MilestoneBase>();

        public abstract bool CheckCompleteMilestone();

        public virtual void SetCompleteMilestone()
        {
            foreach (var unlocked in unlockedObjects)
            {
                unlocked.SetActive(true);
            }
            
            foreach (var fog in removedFog)
            {
                fog.SetActive(false);
            }
        }
    }
}