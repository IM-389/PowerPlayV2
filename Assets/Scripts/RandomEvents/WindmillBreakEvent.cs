using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillBreakEvent : EventBase
{
    public override void DoEvent()
    {
        Debug.Log("Windmill Break Selected!");
        int chance = Random.Range(0, 100);

        if (chance < eventChance)
        {
            Debug.Log("Running Windmill Break!");
            GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");

            List<GeneratorScript> turbines = new List<GeneratorScript>();

            foreach (var generator in generators)
            {
                GeneratorScript gs = generator.GetComponent<GeneratorScript>();
                if (gs.type == PowerManager.POWER_TYPES.TYPE_WIND)
                {
                    turbines.Add(gs);
                }
            }

            int randTurbine = Random.Range(0, turbines.Count);

            StartCoroutine(turbines[randTurbine].TimedDisable(30));
        }
    }
}
