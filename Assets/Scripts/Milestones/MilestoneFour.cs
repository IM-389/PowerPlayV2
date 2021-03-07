using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Milestones;
using UnityEngine;

public class MilestoneFour : MilestoneBase
{
    private int startDay = -1;

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
                    house.GetComponent<StorageScript>().powerStored > 0)
                {
                    // Add to the list of previously powered houses
                    poweredHouses.Add(house);
                }
            }
        }

        // If enough houses are powered, start the week-long cooldown
        if (poweredHouses.Count >= 25)
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

            return daysElapsed >= 7;
        }

        return false;
    }

    public override void SetMilestoneProperties()
    {
        sequenceNumber = 4;
        milestoneName = "Rainy Day";
        milestoneText = "Power all houses for a 2 days";
    }
}
