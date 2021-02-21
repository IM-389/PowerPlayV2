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
    public float timeStep = 0.5f;
    public int totalTimeSteps;
    public int minutes, hours, days = 0;
    public bool isDay = true;
    public int cash = 0;//gonna be using this to cause houses to make money
    // Start is called before the first frame update
    void Start()
    {
        width = 600;
        height = 600;
        rect = new Rect(1800, -250, width, height);
        StartCoroutine("TimeCalculator");//You start the coroutine, it will repeat itself unless you call StopCoroutine("TimeCalculator");
    }
    
    //to increase speed: i'll need a button, and when its clicked, set Time.timeScale to 2F/1.5F
    //When clicked again, set Time.timeScale back to 1F
    public void onButtonPressTwoTimesSpeed()
    {
        Time.timeScale = 2F;
    }
    public void onButtonPressOneAndHalfSpeed()
    {   
        Time.timeScale = 1.5F;
    }
    public void resetToRegSpeed()
    {    
        Time.timeScale = 1F;
    }
    public void pause()
    {
        if(Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator TimeCalculator()//This is a coroutine.
    {
        while (true)
        {
            timeStep = (Time.time% 20) * 2;//timeStep + 0.5;//1.9?
            totalTimeSteps++;
            if (timeStep >= 38)
            {
                hours++;
                timeStep -= timeStep;
            }
            if(hours >= 6 && hours <= 18)
            {
                isDay = true;
                Debug.Log("The time is Day");
            }
            if(hours >=19 && hours <= 24 || hours <=5)
            {
                isDay = false;
                Debug.Log("The time is Night");//can add exact minutes later if they want
            }
            if(hours == 24)
            {
                isDay = true;
                hours = 0;
            }
            //Debug.Log("Timestep: " + timeStep.ToString() + " Hours: " + hours.ToString() + " Total timesteps: " + totalTimeSteps.ToString() + " Daytime: " + isDay);

            yield return new WaitForSeconds(1.0F);//This is the time to wait before the coroutine do its stuff again. There, you put the duration in seconds of an IN GAME minute. Right now, minutes will last for one second, just like it is in Zelda Majora's mask (the N64 version).
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
