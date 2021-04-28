using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyDayEvent : EventBase
{
    private PowerManager powerManager;

    public float powerAdjust;
    
    private void Start()
    {
        base.Start();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }
    
    public override bool DoEvent(bool force)
    {
        Debug.Log("Cloudy Day Selected!");
        int chance = Random.Range(0, 100);

        if (force)
        {
            chance = 0;
        }
        
        if (chance < eventChance)
        {
            Debug.Log("Running Cloudy Day!");
            powerManager.powerAdjusts[2] = powerAdjust;
            return true;
        }

        return false;
    }
}
