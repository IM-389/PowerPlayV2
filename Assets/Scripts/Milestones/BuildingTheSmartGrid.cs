using Power.V2;
using System.Collections.Generic;
using UnityEngine;
using Milestones;

public class BuildingTheSmartGrid : MilestoneBase
{
    //private int startDay = -1;

    private TimeManager timeManager;

    private List<GameObject> poweredHouses = new List<GameObject>();

    private bool startCountdown = false;
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
        if (poweredHouses.Count >= 29)
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
            int daysElapsed = timeManager.days - startDay;

            return daysElapsed >= 1;
        }

        return false;
    }
}
