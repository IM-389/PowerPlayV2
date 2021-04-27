using UnityEngine;

namespace Milestones
{
    public class GettingStartedP1 : MilestoneBase
    {
        private Vector2 startPos;
        private float startZoom;

        private Camera mainCam;

        public GameObject griddie;
        private void Start()
        {
            mainCam = Camera.main;
            startPos = mainCam.transform.position;
            startZoom = mainCam.orthographicSize;
        }
        
        public override bool CheckCompleteMilestone()
        {
            if (griddie.activeSelf)
            {
                return false;
            }
            
            Vector2 currentPos = mainCam.transform.position;
            float currentZoom = mainCam.orthographicSize;

            Vector2 locationDifference = startPos - currentPos;

            return (Mathf.Abs(locationDifference.x) >=  0.5f && Mathf.Abs(locationDifference.y) >= 0.5f)
                && currentZoom != startZoom;
        }
    }
}