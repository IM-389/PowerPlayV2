using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TimeManager : MonoBehaviour
{
    private int width, height;
    private Rect rect;
    private GUIStyle labelStyle;
    private string currentTime;
    //public int minutes = -1, hours, days, years;//This won't wait for the time to pass the very first time it's called, so minus one the minutes.
    public double timeStep = 0;
    public int minutes, hours, days = 0;
    // Start is called before the first frame update
    void Start()
    {
        width = 600;
        height = 600;
        rect = new Rect(1800, -250, width, height);
        StartCoroutine("TimeCalculator");//You start the coroutine, it will repeat itself unless you call StopCoroutine("TimeCalculator");
    }


    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator TimeCalculator()//This is a coroutine.
    {
        while (true)
        {
            timeStep = timeStep + 0.5;//1.9?
            if (timeStep >= 10)
            {
                hours++;
                timeStep -= 10;
            }
            Debug.Log("Timestep: " + timeStep.ToString() + "Hours: " + hours.ToString());//"Days:" + days.ToString() + "  Hours:" + hours.ToString() + "  Minutes:" + minutes.ToString() + " Seconds:" + seconds.ToString());

            yield return new WaitForSeconds(1.0f);//This is the time to wait before the coroutine do its stuff again. There, you put the duration in seconds of an IN GAME minute. Right now, minutes will last for one second, just like it is in Zelda Majora's mask (the N64 version).
        }
    }
    void OnGUI()
    {
        // Display the label at the center of the window.
        labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        labelStyle.alignment = TextAnchor.MiddleCenter;

        // Modify the size of the font based on the window.
        labelStyle.fontSize = 12 * (width / 200);

        // Obtain the current time.
        currentTime = Time.time.ToString("f6");
        currentTime = "Time is: " + currentTime + " sec.";

        // Display the current time.
        GUI.Label(rect, currentTime, labelStyle);
    }
}
