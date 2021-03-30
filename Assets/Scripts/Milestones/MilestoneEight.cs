using Milestones;
using Power.V2;
using UnityEngine;

public class MilestoneEight : MilestoneBase
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
            if (house.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
            {
                ++poweredHouses;
            }
        }

        if (daysElapsed >= 3)
        {
            return true;
        }
        
        if (poweredHouses < allHouses.Length)
        {
            startDay = -1;
        }

        return false;

    }
}
