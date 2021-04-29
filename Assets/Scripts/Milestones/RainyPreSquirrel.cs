using System.Collections;
using System.Collections.Generic;
using Milestones;
using Power.V2;
using UnityEngine;

public class RainyPreSquirrel : MilestoneBase
{
    private RandomEventManager eventManager;

    private bool ranEvent = false;

    private bool delayStarted = false;

    private bool delayFinished = false;
    private void Start()
    {
        eventManager = GameObject.FindObjectOfType<RandomEventManager>();
    }
    
    public override bool CheckCompleteMilestone()
    {
        if (!delayStarted)
        {
            delayStarted = true;
            StartCoroutine(RunDelay());
        }

        if (!delayFinished)
        {
            return false;
        }
        
        if (!ranEvent)
        {
            eventManager.RunEvent(2);
            ranEvent = true;
        }

        return true;
    }

    private IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(10);
        delayFinished = true;
    }
}
