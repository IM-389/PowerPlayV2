using UnityEngine;

namespace Milestones
{
    public class GettingStartedP3 : MilestoneBase
    {
        public GameObject targetPole;

        private Camera mainCam;

        private void Start()
        {
            mainCam = Camera.main;
        }
        
        public override bool CheckCompleteMilestone()
        {
            Vector2 targetScreenPos = mainCam.WorldToScreenPoint(targetPole.transform.position);
            Vector2 targetViewportPos = mainCam.ScreenToViewportPoint(targetScreenPos);

            return (targetViewportPos.x <= 0.6 && targetViewportPos.x >= 0.4) &&
                   (targetViewportPos.y <= 0.6 && targetViewportPos.y >= 0.4);
        }
    }
}