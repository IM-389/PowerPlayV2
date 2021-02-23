using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase : MonoBehaviour
{

    [Tooltip("What is the chance of this event occuring if selected")]
    [Range(0, 100)]
    public int eventChance;

    protected TimeManager timeManager;

    protected void Start()
    {
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
    }
    
    public abstract void DoEvent();

}
