using System.Collections.Generic;
using UnityEngine;
using Power.V2;
using UnityEngine.UI;
namespace Milestones
{
    public class EnterTheSmartGrid : MilestoneBase
    {
        //private int startDay = -1;

        private TimeManager timeManager;

        private List<GameObject> poweredHouses = new List<GameObject>();

        private bool startCountdown = false;

        [Tooltip("Area to upgrade the houses in")]
        public GameObject upgradeArea;

        public Text daysLeft;
        private void Start()
        {
            timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        }
        public override bool CheckCompleteMilestone()
        {
            GameObject[] allHouses = GameObject.FindGameObjectsWithTag("house");

            foreach (var house in allHouses)
            {
                // If the house has not already been counted
                if (!poweredHouses.Contains(house))
                {
                    // If the house is powered
                    if (house.GetComponent<GeneralObjectScript>().isMilestoneCounted &&
                        house.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
                    {
                        // Add to the list of previously powered houses
                        poweredHouses.Add(house);
                    }
                }
            }

            // If enough houses are powered, start the week-long cooldown
            if (poweredHouses.Count >= 22)
            {
                startCountdown = true;
            }

            if (startCountdown)
            {
                // Set the starting day
                if (startDay < 0)
                {
                    startDay = timeManager.days;
                }

                // Count the days elapsed
                daysElapsed = timeManager.days - startDay;
                daysLeft.text = "Days all houses powered: " + daysElapsed;
                return daysElapsed >= 1;
            }

            return false;
        }

        public override void SetCompleteMilestone()
        {
            base.SetCompleteMilestone();

            for (int i = 0; i < upgradeArea.transform.childCount; ++i)
            {
                if (upgradeArea.transform.GetChild(i).CompareTag("house"))
                {
                    upgradeArea.transform.GetChild(i).GetComponent<GeneralObjectScript>().isSmart = true;
                    upgradeArea.transform.GetChild(i).GetChild(5).gameObject.SetActive(true);
                }
            }
        }
    }
}