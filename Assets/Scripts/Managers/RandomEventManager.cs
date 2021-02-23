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

            eventNotification.transform.GetChild(1).GetComponent<Text>().text = eventSelected.notification;
            eventNotification.SetActive(true);
            StartCoroutine(HideNotification());
            eventSelected.DoEvent();
        }

        // Debug code, press the S key for a sunny day, c for cloudy day, and b for windmill break
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < 4; ++i)
            {
                powerManager.powerAdjusts[i] = 1;
            }
            
            eventNotification.transform.GetChild(1).GetComponent<Text>().text = events[0].notification;
            eventNotification.SetActive(true);
            StartCoroutine(HideNotification());
            events[0].DoEvent();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < 4; ++i)
            {
                powerManager.powerAdjusts[i] = 1;
            }
            eventNotification.transform.GetChild(1).GetComponent<Text>().text = events[1].notification;
            eventNotification.SetActive(true);
            StartCoroutine(HideNotification());
            events[1].DoEvent();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            for (int i = 0; i < 4; ++i)
            {
                powerManager.powerAdjusts[i] = 1;
            }
            eventNotification.transform.GetChild(1).GetComponent<Text>().text = events[2].notification;
            eventNotification.SetActive(true);
            StartCoroutine(HideNotification());
            events[2].DoEvent();
        }
        
        previousTimestep = timeManager.days;
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(notificationShowTime);
        eventNotification.SetActive(false);
    }
}
