using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyDayEvent : EventBase
{
    private PowerManager powerManager;

    private void Start()
    {
        base.Start();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }
    
    public override bool DoEvent()
    {
        Debug.Log("Cloudy Day Selected!");
        int chance = Random.Range(0, 100);

        if (chance < eventChance)
        {
            Debug.Log("Running Cloudy Day!");
            powerManager.powerAdjusts[2] = 0.5f;
            return true;
        }

        return false;
    }
}
