using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindlessDayEvent : EventBase
{
    private PowerManager powerManager;

    public float powerAdjust;
    
    private void Start()
    {
        base.Start();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }
    
    public override bool DoEvent()
    {
        Debug.Log("Windless Day Selected!");
        int chance = Random.Range(0, 100);

        if (chance < eventChance)
        {
            Debug.Log("Running Windless Day!");
            powerManager.powerAdjusts[3] = powerAdjust;
            return true;
        }

        return false;
    }
}
