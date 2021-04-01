using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Milestones;
using Power.V2;
using UnityEngine;

public class MilestoneFour : MilestoneBase
{
   // private int startDay = -1;

    private TimeManager timeManager;

    private MilestoneManager mileManager;

    private List<GameObject> poweredHouses = new List<GameObject>();

    //public Text dayLeftText;
    //public Text secondLeftText;

    private bool startCountdown = false;
    private void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        mileManager = GameObject.FindWithTag("GameController").GetComponent<MilestoneManager>();
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
            Debug.Log("Should be in startcountdown now");
            // Count the days elapsed
            daysElapsed = timeManager.days - startDay;
            mileManager.timeText.text = "Days all houses powered: " + daysElapsed ;

            return daysElapsed >= 2;
        }
        
        return false;
    }
}
