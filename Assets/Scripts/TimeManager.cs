using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using Milestones;
public class TimeManager : MonoBehaviour
{
    private int width, height;
    private Rect rect;
    private GUIStyle labelStyle;
    private string currentTime;
    //public int minutes = -1, hours, days, years;//This won't wait for the time to pass the very first time it's called, so minus one the minutes.
    public float timeStep = 0.5f;
    public int totalTimeSteps;
    public int minutes, displayHours, hours = 0;
    public int cityApproval = 0;//Note: we've also got this
    public int days = 1;
    public bool isDay = true;
    public Text clock;
    public int cash = 0;//gonna be using this to cause houses to make money
    ConsumerScript isConsuming;
    MilestoneBase coinGen;
    //Accesses the FMOD Event
    [Tooltip("The location of the sound")]
    [FMODUnity.EventRef]
    public string backgroundReference;
    FMOD.Studio.EventInstance backgrounds;


    // Start is called before the first frame update
    void Start()
    {
        width = 600;
        height = 600;
        rect = new Rect(1800, -250, width, height);
        Debug.Log("Start happened");
        Debug.Log(this.gameObject);
        StartCoroutine("TimeCalculator");//You start the coroutine, it will repeat itself unless you call StopCoroutine("TimeCalculator");

        // Finding and Starting the Event
        backgrounds = FMODUnity.RuntimeManager.CreateInstance(backgroundReference);
        backgrounds.start();
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
        //Changing the music based on TOD
        backgrounds.setParameterByName("Time Of Day", hours);
    }

    
    IEnumerator TimeCalculator()//This is a coroutine.
    {
        while (true)
        {
            timeStep = (Time.time% 20) * 2;//timeStep + 0.5;//1.9?
            totalTimeSteps++;
         
            if (timeStep >= 38)
            {
                timeStep -= timeStep;
                hours++;
                displayHours++;
                //minutes = 0;
               
              
            }
            if(hours >= 1 && hours <= 25)
            {
                //isDay = true;
                string buffer = "";
                if (hours == 25)
                {
                    //isDay = true;
                    hours = 1;
                   // displayHours = 1;
                    ++days;
                }
                if (hours == 12 || hours == 24)
                {
                    buffer += "Day: " + days + ", 12 ";
                    if(hours == 24)
                    {
                        buffer += " A.M";
                    }
                }
                else
                {
                    buffer += "Day: " + days + ", " + hours % 12;
                }

                if (hours >= 12 && hours != 24)
                {
            
                    buffer +=  " P.M";
                }

                else if(hours > 24 || hours <= 11)
                {
                    buffer += " A.M";
                }
                if(hours == 6 || hours == 12 || hours == 16 || hours == 24)
                {
                    if (coinGen != null)
                    {
                        coinGen.smartCoins++;
                    }
                }
                if(hours >= 5 || hours <= 18)
                {
                    isDay = true;
                    Debug.Log("It is daytime");
                }
                else
                {
                    isDay = false;
                    Debug.Log("It is night");
                }
                clock.text = buffer + isDay;
                //Debug.Log("The time is Day");
            }


            
            yield return new WaitForSeconds(0.35F);//This is the time to wait before the coroutine do its stuff again. There, you put the duration in seconds of an IN GAME minute. Right now, minutes will last for one second, just like it is in Zelda Majora's mask (the N64 version).
        }
    }
    /*
    void OnGUI()
    {
        // Display the label at the center of the window.
        labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        labelStyle.alignment = TextAnchor.MiddleCenter;

        // Modify the size of the font based on the window.
        labelStyle.fontSize = 12 * (width / 200);

        // Obtain the current time.
        currentTime = Time.time.ToString("f6");
        currentTime = "Day: " + days + " Hour: " + hours;

        // Display the current time.
        GUI.Label(rect, currentTime, labelStyle);
    }
    */
}
