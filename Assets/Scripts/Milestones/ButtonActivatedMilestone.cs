using UnityEngine.UI;

namespace Milestones
{
    public class ButtonActivatedMilestone : MilestoneBase
    {
        public Button targetButton;

        private bool hasSetListener = false;
        private bool hasClicked = false;

        public override bool CheckCompleteMilestone()
        {
            if (!hasSetListener)
            {
                targetButton.onClick.AddListener(SetClicked);
                hasSetListener = true;
            }

            return hasClicked;
        }

        private void SetClicked()
        {
            hasClicked = true;
        }
        
    }
}