using Power.V2;
using System.Collections.Generic;
using Milestones;
using UnityEngine;
using UnityEngine.UI;

public class Satisfied : MilestoneBase {
    //private int startDay = -1;

    private TimeManager timeManager;
    private RandomEventManager randomEvents;

    private List<GameObject> poweredHouses = new List<GameObject>();

    public Text daysLeft;
    
    private bool startCountdown = false;
    private void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        randomEvents = GameObject.FindWithTag("GameController").GetComponent<RandomEventManager>();
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
        if (poweredHouses.Count >= 23)
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
        randomEvents.doEvents = true;
    }
}
