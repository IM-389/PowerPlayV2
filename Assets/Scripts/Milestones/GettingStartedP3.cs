using UnityEngine;

namespace Milestones
{
    public class GettingStartedP3 : MilestoneBase
    {
        public GameObject targetPole;

        private Camera mainCam;

        private Vector3 targetPos;
        private void Start()
        {
            mainCam = Camera.main;
            targetPos = targetPole.transform.position;
        }
        
        public override bool CheckCompleteMilestone()
        {
            Vector2 targetScreenPos = mainCam.WorldToScreenPoint(targetPos);
            Vector2 targetViewportPos = mainCam.ScreenToViewportPoint(targetScreenPos);

            return (targetViewportPos.x <= 0.7 && targetViewportPos.x >= 0.3) &&
                   (targetViewportPos.y <= 0.7 && targetViewportPos.y >= 0.3);
        }
    }
}