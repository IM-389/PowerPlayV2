using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{

    public List<EventBase> events = new List<EventBase>();
    
    /// <summary>
    /// Reference to the TimeManager. Used to detect timestep changes
    /// </summary>
    protected TimeManager timeManager;
    
    /// <summary>
    /// Current timestep value
    /// </summary>
    private float timestep;

    /// <summary>
    /// Timestep last frame
    /// </summary>
    private float previousTimestep;

    private PowerManager powerManager;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * Once per day an event is selected, and there is a chance that event occurs
         */
        
        // Ticks on day changes
        timestep = timeManager.days;
        if (timestep > previousTimestep)
        {
            // Reset all the power adjustments each day
            for (int i = 0; i < 4; ++i)
            {
                powerManager.powerAdjusts[i] = 1;
            }
            
            // Determine which event to select
            int eventRNG = Random.Range(0, events.Count);

            EventBase eventSelected = events[eventRNG];
            
            eventSelected.DoEvent();
        }
    
        previousTimestep = timeManager.days;
    }
}
