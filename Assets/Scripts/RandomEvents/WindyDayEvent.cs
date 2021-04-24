using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyDayEvent : EventBase
{
    private PowerManager powerManager;

    public float coalAdjust;
    public float solarAdjust;
    public float windAdjust;
    
    private void Start()
    {
        base.Start();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }
    
    public override bool DoEvent()
    {
        Debug.Log("Windy Day Selected!");
        int chance = Random.Range(0, 100);

        if (chance < eventChance)
        {
            Debug.Log("Running Windy Day!");
            StartCoroutine(SunnyDay());//this delays sunny day to only happen in the daytime. keep that in mind. windmill break can and will trigger instantly
            return true;
        }

        return false;
    }

    private IEnumerator SunnyDay()
    {
        // Wait for day to start the event
        while (!timeManager.isDay)
        {
            yield return new WaitForSeconds(0.5f);
        }

        powerManager.powerAdjusts[0] = coalAdjust;
        powerManager.powerAdjusts[1] = 0;
        powerManager.powerAdjusts[2] = solarAdjust;
        powerManager.powerAdjusts[3] = windAdjust;

        yield return new WaitForSeconds(30f);

        for (int i = 0; i < 4; ++i)
        {
            powerManager.powerAdjusts[i] = 1;
        }

    }
}
