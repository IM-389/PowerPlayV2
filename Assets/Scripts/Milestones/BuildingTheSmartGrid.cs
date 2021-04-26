using Power.V2;
using System.Collections.Generic;
using UnityEngine;
using Milestones;
using UnityEngine.UI;

public class BuildingTheSmartGrid : MilestoneBase
{
    //private int startDay = -1;

    private TimeManager timeManager;

    private PowerManager powerManager;
    
    private List<GameObject> poweredHouses = new List<GameObject>();

    [Tooltip("How much power beyond the amount the milestone starts at is required")]
    public float requiredPower;
    
    public Text daysLeft;

    private float startPower;

    private bool cachedPower = false;
    
    private bool startCountdown = false;
    private void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        powerManager = GameObject.FindObjectOfType<PowerManager>();
    }
    
    public override bool CheckCompleteMilestone()
    {
        if (!cachedPower)
        {
            startPower = powerManager.GetTotalAmountGenerated();
            cachedPower = true;
        }
        
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

            // Count the days elapsed
            daysElapsed = timeManager.days - startDay;
            daysLeft.text = "Days all houses powered: " + daysElapsed;
            return daysElapsed >= 1 && powerManager.GetTotalAmountGenerated() >= (startPower + requiredPower);
        }

        return false;
    }
}
