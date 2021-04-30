using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject eventNotification;

    [Tooltip("How long to show the notification for")]
    public float notificationShowTime;

    public bool doEvents = false;
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
        timestep = timeManager.hours;
        if (doEvents && timestep > previousTimestep && timeManager.hours > 1)
        {
            
            // Determine which event to select
            int eventRNG = Random.Range(0, events.Count);

            EventBase eventSelected = events[eventRNG];

            if (eventSelected.DoEvent(false))
            {
                eventNotification.transform.GetChild(1).GetComponent<Text>().text = eventSelected.notification;
                eventNotification.SetActive(true);
                StartCoroutine(HideNotification());
            }

        }
        
        

        previousTimestep = timeManager.hours;
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(notificationShowTime);
        eventNotification.SetActive(false);
    }

    public void RunEvent(int id)
    {
        events[id].DoEvent(true);
    }
}
