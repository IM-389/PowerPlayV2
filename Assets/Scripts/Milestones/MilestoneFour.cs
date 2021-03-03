using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class MilestoneFour : MilestoneBase
{
    private int startDay = -1;

    private TimeManager timeManager;
    
    private void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
    }
    
    public override bool CheckCompleteMilestone()
    {
        if (startDay < 0)
        {
            startDay = timeManager.days;
        }
        
        GameObject[] allHouses = GameObject.FindGameObjectsWithTag("house");

        int poweredHouses = 0;

        int daysElapsed = timeManager.days - startDay;
        
        foreach (var house in allHouses)
        {
            if (house.GetComponent<GeneralObjectScript>().isMilestoneCounted &&
                house.GetComponent<StorageScript>().powerStored > 0)
            {
                ++poweredHouses;
            }
        }

        if (daysElapsed >= 7 && poweredHouses >= 25)
        {
            return true;
        }
        
        if (poweredHouses < allHouses.Length)
        {
            startDay = -1;
        }

        return false;
    }

    public override void SetMilestoneProperties()
    {
        sequenceNumber = 4;
        milestoneName = "Rainy Day";
        milestoneText = "Power all houses for a week";
    }
}
